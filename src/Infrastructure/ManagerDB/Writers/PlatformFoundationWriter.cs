// Copyright (c) 2026 Sergio Hernandez. All rights reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License").
//  You may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

using Common.Application.Exceptions;
using Common.Application.Interfaces;
using Common.Domain.Constants;
using System.Security.Cryptography;
using System.Text;
using TrackHub.Manager.Infrastructure.Entities;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

public sealed class PlatformFoundationWriter(IApplicationDbContext context, ICurrentPrincipal principal) : IPlatformFoundationWriter
{
    private bool CanAccessAllAccounts =>
        principal.PrincipalType == PrincipalType.ServiceClient && !principal.AccountId.HasValue;

    private Guid RequireAccountAccess(Guid accountId)
    {
        if (accountId == Guid.Empty)
        {
            throw new ForbiddenAccessException();
        }

        if (CanAccessAllAccounts || principal.AccountId == accountId || HasActiveSupportGrant(accountId))
        {
            return accountId;
        }

        throw new ForbiddenAccessException();
    }

    private async Task<Driver> GetDriverForWriteAsync(Guid driverId, CancellationToken cancellationToken)
    {
        var entity = await context.Drivers.FirstAsync(x => x.DriverId == driverId, cancellationToken);
        RequireAccountAccess(entity.AccountId);
        return entity;
    }

    public async Task<DriverVm> CreateDriverAsync(DriverDto driver, CancellationToken cancellationToken)
    {
        var entity = new Driver(RequireAccountAccess(driver.AccountId), driver.Name, driver.Phone, driver.DocumentType, driver.DocumentNumber, driver.Active, driver.EmployeeCode, driver.LicenseNumber, driver.LicenseExpiresAt, driver.DefaultTransporterId);
        await context.Drivers.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return ToVm(entity);
    }

    public async Task UpdateDriverAsync(Guid driverId, DriverDto driver, CancellationToken cancellationToken)
    {
        var entity = await GetDriverForWriteAsync(driverId, cancellationToken);
        RequireAccountAccess(entity.AccountId);
        if (driver.AccountId != entity.AccountId)
        {
            throw new ForbiddenAccessException();
        }

        context.Drivers.Attach(entity);
        entity.Name = driver.Name;
        entity.Phone = driver.Phone;
        entity.DocumentType = driver.DocumentType;
        entity.DocumentNumber = driver.DocumentNumber;
        entity.Active = driver.Active;
        entity.EmployeeCode = driver.EmployeeCode;
        entity.LicenseNumber = driver.LicenseNumber;
        entity.LicenseExpiresAt = driver.LicenseExpiresAt;
        entity.DefaultTransporterId = driver.DefaultTransporterId;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeactivateDriverAsync(Guid driverId, CancellationToken cancellationToken)
    {
        var entity = await GetDriverForWriteAsync(driverId, cancellationToken);
        context.Drivers.Attach(entity);
        entity.Active = false;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<AccountFeatureVm> SetAccountFeatureAsync(AccountFeatureDto feature, CancellationToken cancellationToken)
    {
        var accountId = RequireAccountAccess(feature.AccountId);
        var entity = await context.AccountFeatures.FirstOrDefaultAsync(x => x.AccountId == accountId && x.FeatureKey == feature.FeatureKey, cancellationToken);
        string? oldValues = null;
        if (entity == null)
        {
            entity = new AccountFeature(accountId, feature.FeatureKey, feature.Enabled, feature.Tier, feature.Source, feature.EffectiveFrom, feature.EffectiveTo, feature.ConfigurationJson);
            await context.AccountFeatures.AddAsync(entity, cancellationToken);
        }
        else
        {
            oldValues = FeatureAuditValues(entity);
            context.AccountFeatures.Attach(entity);
            entity.Enabled = feature.Enabled;
            entity.Tier = feature.Tier;
            entity.Source = feature.Source;
            entity.EffectiveFrom = feature.EffectiveFrom;
            entity.EffectiveTo = feature.EffectiveTo;
            entity.ConfigurationJson = feature.ConfigurationJson;
        }

        AddAuditEvent(accountId, "SetAccountFeature", "AccountFeature", entity.AccountFeatureId.ToString(), oldValues, FeatureAuditValues(entity));
        await context.SaveChangesAsync(cancellationToken);
        return ToVm(entity);
    }

    public async Task DisableAccountFeatureAsync(Guid accountFeatureId, CancellationToken cancellationToken)
    {
        var entity = await context.AccountFeatures.FirstAsync(x => x.AccountFeatureId == accountFeatureId, cancellationToken);
        RequireAccountAccess(entity.AccountId);
        context.AccountFeatures.Attach(entity);
        var oldValues = FeatureAuditValues(entity);
        entity.Enabled = false;
        AddAuditEvent(entity.AccountId, "DisableAccountFeature", "AccountFeature", entity.AccountFeatureId.ToString(), oldValues, FeatureAuditValues(entity));
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAccountFeatureConfigurationAsync(Guid accountFeatureId, string? configurationJson, CancellationToken cancellationToken)
    {
        var entity = await context.AccountFeatures.FirstAsync(x => x.AccountFeatureId == accountFeatureId, cancellationToken);
        RequireAccountAccess(entity.AccountId);
        context.AccountFeatures.Attach(entity);
        var oldValues = FeatureAuditValues(entity);
        entity.ConfigurationJson = configurationJson;
        AddAuditEvent(entity.AccountId, "UpdateAccountFeatureConfiguration", "AccountFeature", entity.AccountFeatureId.ToString(), oldValues, FeatureAuditValues(entity));
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<AuditEventVm> CreateAuditEventAsync(AuditEventDto auditEvent, CancellationToken cancellationToken)
    {
        var entity = new AuditEvent(RequireAccountAccess(auditEvent.AccountId), auditEvent.ActorType, auditEvent.ActorId, auditEvent.Action, auditEvent.ResourceType, auditEvent.ResourceId, auditEvent.Result, auditEvent.OldValuesJson, auditEvent.NewValuesJson, auditEvent.Reason, auditEvent.IpAddress, auditEvent.UserAgent, auditEvent.CorrelationId);
        await context.AuditEvents.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return ToVm(entity);
    }

    public async Task<DocumentVm> CreateDocumentMetadataAsync(DocumentDto document, CancellationToken cancellationToken)
    {
        var entity = new Document(RequireAccountAccess(document.AccountId), document.OwnerEntityType, document.OwnerEntityId, document.UploadedByPrincipalType, document.UploadedByPrincipalId, document.StorageProvider, document.StorageKey, document.ContentType, document.SizeBytes, document.Sha256Hash, document.Classification, document.Status, document.ExpiresAt, document.VisibilityScope, document.ScanStatus);
        await context.Documents.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return ToVm(entity);
    }

    public async Task MarkDocumentUploadedAsync(Guid documentId, string status, CancellationToken cancellationToken)
    {
        var entity = await context.Documents.FirstAsync(x => x.DocumentId == documentId, cancellationToken);
        RequireAccountAccess(entity.AccountId);
        context.Documents.Attach(entity);
        entity.Status = status;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkDocumentScanResultAsync(Guid documentId, string scanStatus, CancellationToken cancellationToken)
    {
        var entity = await context.Documents.FirstAsync(x => x.DocumentId == documentId, cancellationToken);
        RequireAccountAccess(entity.AccountId);
        context.Documents.Attach(entity);
        entity.ScanStatus = scanStatus;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task ExpireDocumentAsync(Guid documentId, DateTimeOffset expiresAt, CancellationToken cancellationToken)
    {
        var entity = await context.Documents.FirstAsync(x => x.DocumentId == documentId, cancellationToken);
        RequireAccountAccess(entity.AccountId);
        context.Documents.Attach(entity);
        entity.ExpiresAt = expiresAt;
        entity.Status = "Expired";
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteDocumentReferenceAsync(Guid documentId, CancellationToken cancellationToken)
    {
        var entity = await context.Documents.FirstAsync(x => x.DocumentId == documentId, cancellationToken);
        RequireAccountAccess(entity.AccountId);
        context.Documents.Attach(entity);
        entity.Status = "Deleted";
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<NotificationRuleVm> CreateNotificationRuleAsync(NotificationRuleDto notificationRule, CancellationToken cancellationToken)
    {
        var entity = new NotificationRule(RequireAccountAccess(notificationRule.AccountId), notificationRule.RuleKey, notificationRule.RuleType, notificationRule.Enabled, notificationRule.TriggerEvent, notificationRule.RecipientSelector, notificationRule.ChannelsJson, notificationRule.ThrottlingJson, notificationRule.ConfigurationJson);
        await context.NotificationRules.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return ToVm(entity);
    }

    public async Task UpdateNotificationRuleAsync(Guid notificationRuleId, NotificationRuleDto notificationRule, CancellationToken cancellationToken)
    {
        var entity = await context.NotificationRules.FirstAsync(x => x.NotificationRuleId == notificationRuleId, cancellationToken);
        RequireAccountAccess(entity.AccountId);
        if (notificationRule.AccountId != entity.AccountId)
        {
            throw new ForbiddenAccessException();
        }

        context.NotificationRules.Attach(entity);
        entity.RuleKey = notificationRule.RuleKey;
        entity.RuleType = notificationRule.RuleType;
        entity.Enabled = notificationRule.Enabled;
        entity.TriggerEvent = notificationRule.TriggerEvent;
        entity.RecipientSelector = notificationRule.RecipientSelector;
        entity.ChannelsJson = notificationRule.ChannelsJson;
        entity.ThrottlingJson = notificationRule.ThrottlingJson;
        entity.ConfigurationJson = notificationRule.ConfigurationJson;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DisableNotificationRuleAsync(Guid notificationRuleId, CancellationToken cancellationToken)
    {
        var entity = await context.NotificationRules.FirstAsync(x => x.NotificationRuleId == notificationRuleId, cancellationToken);
        RequireAccountAccess(entity.AccountId);
        context.NotificationRules.Attach(entity);
        entity.Enabled = false;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<AlertEventVm> RecordAlertEventAsync(AlertEventDto alertEvent, CancellationToken cancellationToken)
    {
        var accountId = RequireAccountAccess(alertEvent.AccountId);
        var entity = await context.AlertEvents.FirstOrDefaultAsync(x => x.AccountId == accountId && x.DeduplicationKey == alertEvent.DeduplicationKey && x.Status != "Resolved", cancellationToken);
        if (entity == null)
        {
            entity = new AlertEvent(accountId, alertEvent.EventType, alertEvent.Severity, alertEvent.SourceModule, alertEvent.ResourceType, alertEvent.ResourceId, alertEvent.Status, alertEvent.PayloadJson, alertEvent.DeduplicationKey);
            await context.AlertEvents.AddAsync(entity, cancellationToken);
        }
        else
        {
            context.AlertEvents.Attach(entity);
            entity.LastSeenAt = DateTimeOffset.UtcNow;
            entity.PayloadJson = alertEvent.PayloadJson;
        }

        await context.SaveChangesAsync(cancellationToken);
        return ToVm(entity);
    }

    public async Task AcknowledgeAlertEventAsync(Guid alertEventId, CancellationToken cancellationToken)
    {
        var entity = await context.AlertEvents.FirstAsync(x => x.AlertEventId == alertEventId, cancellationToken);
        RequireAccountAccess(entity.AccountId);
        context.AlertEvents.Attach(entity);
        entity.Status = "Acknowledged";
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task ResolveAlertEventAsync(Guid alertEventId, CancellationToken cancellationToken)
    {
        var entity = await context.AlertEvents.FirstAsync(x => x.AlertEventId == alertEventId, cancellationToken);
        RequireAccountAccess(entity.AccountId);
        context.AlertEvents.Attach(entity);
        entity.Status = "Resolved";
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<NotificationDeliveryVm> CreateNotificationDeliveryAsync(NotificationDeliveryDto notificationDelivery, CancellationToken cancellationToken)
    {
        var entity = new NotificationDelivery(RequireAccountAccess(notificationDelivery.AccountId), notificationDelivery.NotificationRuleId, notificationDelivery.AlertEventId, notificationDelivery.Channel, notificationDelivery.RecipientPrincipalType, notificationDelivery.Recipient, notificationDelivery.Status);
        await context.NotificationDeliveries.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return ToVm(entity);
    }

    public async Task<BackgroundJobRunVm> CreateBackgroundJobRunAsync(BackgroundJobRunDto backgroundJobRun, CancellationToken cancellationToken)
    {
        Guid? accountId = backgroundJobRun.AccountId.HasValue
            ? RequireAccountAccess(backgroundJobRun.AccountId.Value)
            : CanAccessAllAccounts ? null : throw new ForbiddenAccessException();
        var entity = new BackgroundJobRun(backgroundJobRun.JobKey, accountId, backgroundJobRun.ResourceKey, backgroundJobRun.IdempotencyKey, backgroundJobRun.Status, backgroundJobRun.Attempts, backgroundJobRun.StartedAt)
        {
            CompletedAt = backgroundJobRun.CompletedAt,
            ErrorCode = backgroundJobRun.ErrorCode,
            ErrorMessage = backgroundJobRun.ErrorMessage
        };
        await context.BackgroundJobRuns.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return ToVm(entity);
    }

    public async Task<PublicLinkGrantVm> CreatePublicLinkGrantAsync(PublicLinkGrantDto publicLinkGrant, CancellationToken cancellationToken)
    {
        var token = GeneratePublicLinkToken();
        var subjectTokenIdHash = string.IsNullOrWhiteSpace(publicLinkGrant.SubjectTokenIdHash)
            ? HashPublicLinkToken(token)
            : publicLinkGrant.SubjectTokenIdHash;
        var entity = new PublicLinkGrant(RequireAccountAccess(publicLinkGrant.AccountId), publicLinkGrant.ResourceType, publicLinkGrant.ResourceId, publicLinkGrant.Scopes, publicLinkGrant.Purpose, subjectTokenIdHash, publicLinkGrant.ExpiresAt, publicLinkGrant.CreatedByPrincipalId);
        await context.PublicLinkGrants.AddAsync(entity, cancellationToken);
        AddAuditEvent(entity.AccountId, "CreatePublicLinkGrant", "PublicLinkGrant", entity.PublicLinkGrantId.ToString(), null, PublicLinkAuditValues(entity));
        await context.SaveChangesAsync(cancellationToken);
        return ToVm(entity, string.IsNullOrWhiteSpace(publicLinkGrant.SubjectTokenIdHash) ? token : null);
    }

    public async Task RevokePublicLinkGrantAsync(Guid publicLinkGrantId, string revokedBy, CancellationToken cancellationToken)
    {
        var entity = await context.PublicLinkGrants.FirstAsync(x => x.PublicLinkGrantId == publicLinkGrantId, cancellationToken);
        RequireAccountAccess(entity.AccountId);
        context.PublicLinkGrants.Attach(entity);
        var oldValues = PublicLinkAuditValues(entity);
        entity.RevokedAt = DateTimeOffset.UtcNow;
        entity.RevokedBy = revokedBy;
        AddAuditEvent(entity.AccountId, "RevokePublicLinkGrant", "PublicLinkGrant", entity.PublicLinkGrantId.ToString(), oldValues, PublicLinkAuditValues(entity));
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RecordPublicLinkAccessAsync(Guid publicLinkGrantId, CancellationToken cancellationToken)
    {
        var entity = await context.PublicLinkGrants.FirstAsync(x => x.PublicLinkGrantId == publicLinkGrantId, cancellationToken);
        RequireAccountAccess(entity.AccountId);
        context.PublicLinkGrants.Attach(entity);
        var oldValues = PublicLinkAuditValues(entity);
        entity.AccessCount++;
        entity.LastAccessedAt = DateTimeOffset.UtcNow;
        AddAuditEvent(entity.AccountId, "RecordPublicLinkAccess", "PublicLinkGrant", entity.PublicLinkGrantId.ToString(), oldValues, PublicLinkAuditValues(entity));
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<AccountSupportGrantVm> CreateAccountSupportGrantAsync(AccountSupportGrantDto accountSupportGrant, CancellationToken cancellationToken)
    {
        var entity = new AccountSupportGrant(RequireAccountAccess(accountSupportGrant.AccountId), accountSupportGrant.SupportUserId, accountSupportGrant.Reason, accountSupportGrant.TicketReference, accountSupportGrant.AccessLevel, accountSupportGrant.StartsAt, accountSupportGrant.EndsAt);
        await context.AccountSupportGrants.AddAsync(entity, cancellationToken);
        AddAuditEvent(entity.AccountId, "CreateAccountSupportGrant", "AccountSupportGrant", entity.AccountSupportGrantId.ToString(), null, SupportGrantAuditValues(entity));
        await context.SaveChangesAsync(cancellationToken);
        return ToVm(entity);
    }

    public async Task ApproveAccountSupportGrantAsync(Guid accountSupportGrantId, string approvedBy, CancellationToken cancellationToken)
    {
        var entity = await context.AccountSupportGrants.FirstAsync(x => x.AccountSupportGrantId == accountSupportGrantId, cancellationToken);
        RequireAccountAccess(entity.AccountId);
        context.AccountSupportGrants.Attach(entity);
        var oldValues = SupportGrantAuditValues(entity);
        entity.ApprovedBy = approvedBy;
        entity.ApprovedAt = DateTimeOffset.UtcNow;
        AddAuditEvent(entity.AccountId, "ApproveAccountSupportGrant", "AccountSupportGrant", entity.AccountSupportGrantId.ToString(), oldValues, SupportGrantAuditValues(entity));
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RevokeAccountSupportGrantAsync(Guid accountSupportGrantId, string revokedBy, CancellationToken cancellationToken)
    {
        var entity = await context.AccountSupportGrants.FirstAsync(x => x.AccountSupportGrantId == accountSupportGrantId, cancellationToken);
        RequireAccountAccess(entity.AccountId);
        context.AccountSupportGrants.Attach(entity);
        var oldValues = SupportGrantAuditValues(entity);
        entity.RevokedBy = revokedBy;
        entity.RevokedAt = DateTimeOffset.UtcNow;
        AddAuditEvent(entity.AccountId, "RevokeAccountSupportGrant", "AccountSupportGrant", entity.AccountSupportGrantId.ToString(), oldValues, SupportGrantAuditValues(entity));
        await context.SaveChangesAsync(cancellationToken);
    }

    private static DriverVm ToVm(Driver x) => new(x.DriverId, x.AccountId, x.Name, x.Phone, x.DocumentType, x.DocumentNumber, x.Active, x.EmployeeCode, x.LicenseNumber, x.LicenseExpiresAt, x.DefaultTransporterId, x.LastModified);
    private static AccountFeatureVm ToVm(AccountFeature x) => new(x.AccountFeatureId, x.AccountId, x.FeatureKey, x.Enabled, x.Tier, x.Source, x.EffectiveFrom, x.EffectiveTo, x.ConfigurationJson, x.LastModified);
    private static AuditEventVm ToVm(AuditEvent x) => new(x.AuditEventId, x.AccountId, x.ActorType, x.ActorId, x.Action, x.ResourceType, x.ResourceId, x.Result, x.Reason, x.IpAddress, x.UserAgent, x.CorrelationId, x.OccurredAt);
    private static DocumentVm ToVm(Document x) => new(x.DocumentId, x.AccountId, x.OwnerEntityType, x.OwnerEntityId, x.UploadedByPrincipalType, x.UploadedByPrincipalId, x.StorageProvider, x.ContentType, x.SizeBytes, x.Sha256Hash, x.Classification, x.Status, x.ExpiresAt, x.VisibilityScope, x.ScanStatus, x.LastModified);
    private static NotificationRuleVm ToVm(NotificationRule x) => new(x.NotificationRuleId, x.AccountId, x.RuleKey, x.RuleType, x.Enabled, x.TriggerEvent, x.RecipientSelector, x.ChannelsJson, x.ThrottlingJson, x.ConfigurationJson, x.LastModified);
    private static AlertEventVm ToVm(AlertEvent x) => new(x.AlertEventId, x.AccountId, x.EventType, x.Severity, x.SourceModule, x.ResourceType, x.ResourceId, x.Status, x.FirstSeenAt, x.LastSeenAt, x.PayloadJson, x.DeduplicationKey, x.LastModified);
    private static NotificationDeliveryVm ToVm(NotificationDelivery x) => new(x.NotificationDeliveryId, x.AccountId, x.NotificationRuleId, x.AlertEventId, x.Channel, x.RecipientPrincipalType, x.Recipient, x.Status, x.Attempts, x.ProviderMessageId, x.Error, x.SentAt, x.ReadAt, x.LastModified);
    private static BackgroundJobRunVm ToVm(BackgroundJobRun x) => new(x.BackgroundJobRunId, x.JobKey, x.AccountId, x.ResourceKey, x.IdempotencyKey, x.Status, x.Attempts, x.StartedAt, x.CompletedAt, x.ErrorCode, x.ErrorMessage);
    private static PublicLinkGrantVm ToVm(PublicLinkGrant x, string? token = null) => new(x.PublicLinkGrantId, x.AccountId, x.ResourceType, x.ResourceId, x.Scopes, x.Purpose, x.ExpiresAt, x.RevokedAt, x.RevokedBy, x.CreatedByPrincipalId, x.AccessCount, x.LastAccessedAt, x.LastModified, token);
    private static AccountSupportGrantVm ToVm(AccountSupportGrant x) => new(x.AccountSupportGrantId, x.AccountId, x.SupportUserId, x.Reason, x.TicketReference, x.ApprovedBy, x.ApprovedAt, x.AccessLevel, x.StartsAt, x.EndsAt, x.RevokedAt, x.RevokedBy, x.LastModified);

    private static string GeneratePublicLinkToken()
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    private static string HashPublicLinkToken(string token)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));

    private void AddAuditEvent(Guid accountId, string action, string resourceType, string resourceId, string? oldValuesJson, string? newValuesJson)
    {
        var actorId = principal.UserId?.ToString()
            ?? principal.DriverId?.ToString()
            ?? principal.ClientId
            ?? principal.SubjectId
            ?? "unknown";

        context.AuditEvents.Add(new AuditEvent(
            accountId,
            principal.PrincipalType.ToString(),
            actorId,
            action,
            resourceType,
            resourceId,
            "Succeeded",
            oldValuesJson,
            newValuesJson,
            null,
            null,
            null,
            principal.CorrelationId));
    }

    private static string FeatureAuditValues(AccountFeature feature)
        => $$"""{"featureKey":"{{feature.FeatureKey}}","enabled":{{feature.Enabled.ToString().ToLowerInvariant()}},"tier":"{{feature.Tier}}","source":"{{feature.Source}}","effectiveFrom":{{Quote(feature.EffectiveFrom)}},"effectiveTo":{{Quote(feature.EffectiveTo)}},"configurationJson":{{Quote(feature.ConfigurationJson)}}}""";

    private static string PublicLinkAuditValues(PublicLinkGrant grant)
        => $$"""{"resourceType":"{{grant.ResourceType}}","resourceId":"{{grant.ResourceId}}","scopes":"{{grant.Scopes}}","purpose":"{{grant.Purpose}}","expiresAt":{{Quote(grant.ExpiresAt)}},"revokedAt":{{Quote(grant.RevokedAt)}},"revokedBy":{{Quote(grant.RevokedBy)}},"accessCount":{{grant.AccessCount}},"lastAccessedAt":{{Quote(grant.LastAccessedAt)}}}""";

    private static string SupportGrantAuditValues(AccountSupportGrant grant)
        => $$"""{"supportUserId":"{{grant.SupportUserId}}","reason":"{{grant.Reason}}","ticketReference":"{{grant.TicketReference}}","approvedBy":{{Quote(grant.ApprovedBy)}},"approvedAt":{{Quote(grant.ApprovedAt)}},"accessLevel":"{{grant.AccessLevel}}","startsAt":{{Quote(grant.StartsAt)}},"endsAt":{{Quote(grant.EndsAt)}},"revokedAt":{{Quote(grant.RevokedAt)}},"revokedBy":{{Quote(grant.RevokedBy)}}}""";

    private static string Quote(DateTimeOffset? value)
        => value.HasValue ? Quote(value.Value.ToString("O")) : "null";

    private static string Quote(string? value)
        => value == null ? "null" : $"\"{value.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";

    private bool HasActiveSupportGrant(Guid accountId)
    {
        if (!principal.UserId.HasValue || !string.Equals(principal.Role, Roles.Administrator, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var now = DateTimeOffset.UtcNow;
        return context.AccountSupportGrants.Any(x =>
            x.AccountId == accountId
            && x.SupportUserId == principal.UserId.Value
            && x.ApprovedAt.HasValue
            && x.StartsAt <= now
            && x.EndsAt >= now
            && !x.RevokedAt.HasValue);
    }
}
