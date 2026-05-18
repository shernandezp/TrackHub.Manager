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
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class PlatformFoundationReader(IApplicationDbContext context, ICurrentPrincipal principal) : IPlatformFoundationReader
{
    private static int PageSize(int take) => Math.Clamp(take <= 0 ? 50 : take, 1, 500);
    private static int Offset(int skip) => Math.Max(0, skip);
    private bool CanAccessAllAccounts =>
        principal.PrincipalType == PrincipalType.ServiceClient && !principal.AccountId.HasValue;

    private Guid RequireAccountAccess(Guid accountId)
    {
        return accountId == Guid.Empty
            ? throw new ForbiddenAccessException()
            : CanAccessAllAccounts || principal.AccountId == accountId || HasActiveSupportGrant(accountId)
            ? accountId
            : throw new ForbiddenAccessException();
    }

    private Guid? ResolveAccountScope(Guid? requestedAccountId)
    {
        if (CanAccessAllAccounts)
        {
            return requestedAccountId;
        }

        if (!principal.AccountId.HasValue)
        {
            throw new ForbiddenAccessException();
        }

        if (requestedAccountId.HasValue && requestedAccountId.Value != principal.AccountId.Value)
        {
            throw new ForbiddenAccessException();
        }

        return principal.AccountId.Value;
    }

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

    public async Task<DriverVm> GetDriverAsync(Guid driverId, CancellationToken cancellationToken)
    {
        var accountId = ResolveAccountScope(null);
        return await context.Drivers
            .Where(x => x.DriverId == driverId && (!accountId.HasValue || x.AccountId == accountId.Value))
            .Select(x => new DriverVm(x.DriverId, x.AccountId, x.Name, x.Phone, x.DocumentType, x.DocumentNumber, x.Active, x.EmployeeCode, x.LicenseNumber, x.LicenseExpiresAt, x.DefaultTransporterId, x.LastModified))
            .FirstAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<DriverVm>> GetDriversByAccountAsync(Guid accountId, int skip, int take, CancellationToken cancellationToken)
        => await context.Drivers
            .Where(x => x.AccountId == RequireAccountAccess(accountId))
            .OrderBy(x => x.Name).ThenBy(x => x.DriverId)
            .Skip(Offset(skip)).Take(PageSize(take))
            .Select(x => new DriverVm(x.DriverId, x.AccountId, x.Name, x.Phone, x.DocumentType, x.DocumentNumber, x.Active, x.EmployeeCode, x.LicenseNumber, x.LicenseExpiresAt, x.DefaultTransporterId, x.LastModified))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<DriverAssignmentVm>> GetDriverAssignmentsAsync(Guid driverId, CancellationToken cancellationToken)
    {
        var accountId = ResolveAccountScope(null);
        var driver = await context.Drivers
            .Where(x => x.DriverId == driverId && (!accountId.HasValue || x.AccountId == accountId.Value))
            .Select(x => new { x.DriverId, x.AccountId, x.DefaultTransporterId, x.Active })
            .FirstAsync(cancellationToken);

        return driver.DefaultTransporterId.HasValue
            ? [new DriverAssignmentVm(driver.DriverId, driver.AccountId, "Transporter", driver.DefaultTransporterId.Value.ToString(), driver.Active)]
            : [];
    }

    public async Task<bool> ValidateDriverAssignmentAsync(Guid driverId, string resourceType, string resourceId, CancellationToken cancellationToken)
    {
        if (!string.Equals(resourceType, "Transporter", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!Guid.TryParse(resourceId, out var parsedResourceId))
        {
            return false;
        }

        var accountId = ResolveAccountScope(null);
        return await context.Drivers.AnyAsync(x =>
            x.DriverId == driverId
            && x.Active
            && (!accountId.HasValue || x.AccountId == accountId.Value)
            && x.DefaultTransporterId == parsedResourceId,
            cancellationToken);
    }

    public async Task<bool> ValidateGroupVisibilityAsync(Guid accountId, Guid userId, string resourceType, string resourceId, CancellationToken cancellationToken)
    {
        var scopedAccountId = RequireAccountAccess(accountId);

        if (!CanAccessAllAccounts && principal.UserId.HasValue && principal.UserId.Value != userId)
        {
            throw new ForbiddenAccessException();
        }

        if (!Guid.TryParse(resourceId, out var parsedResourceId))
        {
            return false;
        }

        if (!string.Equals(resourceType, "Transporter", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return await context.Users
            .Where(user => user.UserId == userId && user.AccountId == scopedAccountId && user.Active)
            .SelectMany(user => user.Groups.Select(group => group.GroupId))
            .Intersect(context.Transporters
                .Where(transporter => transporter.TransporterId == parsedResourceId && transporter.AccountId == scopedAccountId)
                .SelectMany(transporter => transporter.Groups.Select(group => group.GroupId)))
            .AnyAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<AccountFeatureVm>> GetAccountFeaturesAsync(Guid accountId, CancellationToken cancellationToken)
        => await context.AccountFeatures
            .Where(x => x.AccountId == RequireAccountAccess(accountId))
            .OrderBy(x => x.FeatureKey)
            .Select(x => new AccountFeatureVm(x.AccountFeatureId, x.AccountId, x.FeatureKey, x.Enabled, x.Tier, x.Source, x.EffectiveFrom, x.EffectiveTo, x.ConfigurationJson, x.LastModified))
            .ToListAsync(cancellationToken);

    public async Task<bool> ValidateFeatureEnabledAsync(Guid accountId, string featureKey, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var scopedAccountId = RequireAccountAccess(accountId);
        return await context.AccountFeatures.AnyAsync(x =>
            x.AccountId == scopedAccountId
            && x.FeatureKey == featureKey
            && x.Enabled
            && (!x.EffectiveFrom.HasValue || x.EffectiveFrom <= now)
            && (!x.EffectiveTo.HasValue || x.EffectiveTo >= now), cancellationToken);
    }

    public async Task<IReadOnlyCollection<AuditEventVm>> GetAuditTrailAsync(Guid accountId, DateTimeOffset? from, DateTimeOffset? to, int skip, int take, CancellationToken cancellationToken)
        => await context.AuditEvents
            .Where(x => x.AccountId == RequireAccountAccess(accountId) && (!from.HasValue || x.OccurredAt >= from) && (!to.HasValue || x.OccurredAt <= to))
            .OrderByDescending(x => x.OccurredAt).ThenBy(x => x.AuditEventId)
            .Skip(Offset(skip)).Take(PageSize(take))
            .Select(x => new AuditEventVm(x.AuditEventId, x.AccountId, x.ActorType, x.ActorId, x.Action, x.ResourceType, x.ResourceId, x.Result, x.Reason, x.IpAddress, x.UserAgent, x.CorrelationId, x.OccurredAt))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<DocumentVm>> GetDocumentsForOwnerAsync(Guid accountId, string ownerEntityType, string ownerEntityId, DateTimeOffset? from, DateTimeOffset? to, int skip, int take, CancellationToken cancellationToken)
        => await context.Documents
            .Where(x => x.AccountId == RequireAccountAccess(accountId) && x.OwnerEntityType == ownerEntityType && x.OwnerEntityId == ownerEntityId && (!from.HasValue || x.LastModified >= from) && (!to.HasValue || x.LastModified <= to))
            .OrderByDescending(x => x.LastModified).ThenBy(x => x.DocumentId)
            .Skip(Offset(skip)).Take(PageSize(take))
            .Select(x => new DocumentVm(x.DocumentId, x.AccountId, x.OwnerEntityType, x.OwnerEntityId, x.UploadedByPrincipalType, x.UploadedByPrincipalId, x.StorageProvider, x.ContentType, x.SizeBytes, x.Sha256Hash, x.Classification, x.Status, x.ExpiresAt, x.VisibilityScope, x.ScanStatus, x.LastModified))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<NotificationRuleVm>> GetNotificationRulesAsync(Guid accountId, int skip, int take, CancellationToken cancellationToken)
        => await context.NotificationRules
            .Where(x => x.AccountId == RequireAccountAccess(accountId))
            .OrderBy(x => x.RuleKey).ThenBy(x => x.NotificationRuleId)
            .Skip(Offset(skip)).Take(PageSize(take))
            .Select(x => new NotificationRuleVm(x.NotificationRuleId, x.AccountId, x.RuleKey, x.RuleType, x.Enabled, x.TriggerEvent, x.RecipientSelector, x.ChannelsJson, x.ThrottlingJson, x.ConfigurationJson, x.LastModified))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<AlertEventVm>> GetAlertEventsAsync(Guid accountId, DateTimeOffset? from, DateTimeOffset? to, int skip, int take, CancellationToken cancellationToken)
        => await context.AlertEvents
            .Where(x => x.AccountId == RequireAccountAccess(accountId) && (!from.HasValue || x.LastSeenAt >= from) && (!to.HasValue || x.LastSeenAt <= to))
            .OrderByDescending(x => x.LastSeenAt).ThenBy(x => x.AlertEventId)
            .Skip(Offset(skip)).Take(PageSize(take))
            .Select(x => new AlertEventVm(x.AlertEventId, x.AccountId, x.EventType, x.Severity, x.SourceModule, x.ResourceType, x.ResourceId, x.Status, x.FirstSeenAt, x.LastSeenAt, x.PayloadJson, x.DeduplicationKey, x.LastModified))
            .ToListAsync(cancellationToken);

    public async Task<PublicLinkGrantVm> GetPublicLinkGrantAsync(Guid publicLinkGrantId, CancellationToken cancellationToken)
    {
        var accountId = ResolveAccountScope(null);
        return await context.PublicLinkGrants
            .Where(x => x.PublicLinkGrantId == publicLinkGrantId && (!accountId.HasValue || x.AccountId == accountId.Value))
            .Select(x => new PublicLinkGrantVm(x.PublicLinkGrantId, x.AccountId, x.ResourceType, x.ResourceId, x.Scopes, x.Purpose, x.ExpiresAt, x.RevokedAt, x.RevokedBy, x.CreatedByPrincipalId, x.AccessCount, x.LastAccessedAt, x.LastModified, null))
            .FirstAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<BackgroundJobRunVm>> GetBackgroundJobRunsAsync(Guid? accountId, DateTimeOffset? from, DateTimeOffset? to, int skip, int take, CancellationToken cancellationToken)
    {
        var scopedAccountId = ResolveAccountScope(accountId);
        return await context.BackgroundJobRuns
            .Where(x => (!scopedAccountId.HasValue || x.AccountId == scopedAccountId) && (!from.HasValue || x.StartedAt >= from) && (!to.HasValue || x.StartedAt <= to))
            .OrderByDescending(x => x.StartedAt).ThenBy(x => x.BackgroundJobRunId)
            .Skip(Offset(skip)).Take(PageSize(take))
            .Select(x => new BackgroundJobRunVm(x.BackgroundJobRunId, x.JobKey, x.AccountId, x.ResourceKey, x.IdempotencyKey, x.Status, x.Attempts, x.StartedAt, x.CompletedAt, x.ErrorCode, x.ErrorMessage))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<PublicLinkGrantVm>> GetPublicLinkGrantsByAccountAsync(Guid accountId, int skip, int take, CancellationToken cancellationToken)
        => await context.PublicLinkGrants
            .Where(x => x.AccountId == RequireAccountAccess(accountId))
            .OrderByDescending(x => x.LastModified).ThenBy(x => x.PublicLinkGrantId)
            .Skip(Offset(skip)).Take(PageSize(take))
            .Select(x => new PublicLinkGrantVm(x.PublicLinkGrantId, x.AccountId, x.ResourceType, x.ResourceId, x.Scopes, x.Purpose, x.ExpiresAt, x.RevokedAt, x.RevokedBy, x.CreatedByPrincipalId, x.AccessCount, x.LastAccessedAt, x.LastModified, null))
            .ToListAsync(cancellationToken);

    public async Task<AccountSupportGrantVm> GetSupportGrantStatusAsync(Guid accountSupportGrantId, CancellationToken cancellationToken)
    {
        var accountId = ResolveAccountScope(null);
        return await context.AccountSupportGrants
            .Where(x => x.AccountSupportGrantId == accountSupportGrantId && (!accountId.HasValue || x.AccountId == accountId.Value))
            .Select(x => new AccountSupportGrantVm(x.AccountSupportGrantId, x.AccountId, x.SupportUserId, x.Reason, x.TicketReference, x.ApprovedBy, x.ApprovedAt, x.AccessLevel, x.StartsAt, x.EndsAt, x.RevokedAt, x.RevokedBy, x.LastModified))
            .FirstAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<AccountSupportGrantVm>> GetAccountSupportGrantsAsync(Guid? accountId, int skip, int take, CancellationToken cancellationToken)
    {
        var scopedAccountId = ResolveAccountScope(accountId);
        return await context.AccountSupportGrants
            .Where(x => !scopedAccountId.HasValue || x.AccountId == scopedAccountId.Value)
            .OrderByDescending(x => x.LastModified).ThenBy(x => x.AccountSupportGrantId)
            .Skip(Offset(skip)).Take(PageSize(take))
            .Select(x => new AccountSupportGrantVm(x.AccountSupportGrantId, x.AccountId, x.SupportUserId, x.Reason, x.TicketReference, x.ApprovedBy, x.ApprovedAt, x.AccessLevel, x.StartsAt, x.EndsAt, x.RevokedAt, x.RevokedBy, x.LastModified))
            .ToListAsync(cancellationToken);
    }
}
