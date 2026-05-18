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

using Common.Infrastructure;

namespace TrackHub.Manager.Infrastructure.Entities;

public sealed class Driver(
    Guid accountId,
    string name,
    string? phone,
    string? documentType,
    string? documentNumber,
    bool active,
    string? employeeCode,
    string? licenseNumber,
    DateOnly? licenseExpiresAt,
    Guid? defaultTransporterId) : BaseAuditableEntity
{
    public Guid DriverId { get; private set; } = Guid.NewGuid();
    public Guid AccountId { get; set; } = accountId;
    public string Name { get; set; } = name;
    public string? Phone { get; set; } = phone;
    public string? DocumentType { get; set; } = documentType;
    public string? DocumentNumber { get; set; } = documentNumber;
    public bool Active { get; set; } = active;
    public string? EmployeeCode { get; set; } = employeeCode;
    public string? LicenseNumber { get; set; } = licenseNumber;
    public DateOnly? LicenseExpiresAt { get; set; } = licenseExpiresAt;
    public Guid? DefaultTransporterId { get; set; } = defaultTransporterId;
}

public sealed class AccountFeature(
    Guid accountId,
    string featureKey,
    bool enabled,
    string tier,
    string source,
    DateTimeOffset? effectiveFrom,
    DateTimeOffset? effectiveTo,
    string? configurationJson) : BaseAuditableEntity
{
    public Guid AccountFeatureId { get; private set; } = Guid.NewGuid();
    public Guid AccountId { get; set; } = accountId;
    public string FeatureKey { get; set; } = featureKey;
    public bool Enabled { get; set; } = enabled;
    public string Tier { get; set; } = tier;
    public string Source { get; set; } = source;
    public DateTimeOffset? EffectiveFrom { get; set; } = effectiveFrom;
    public DateTimeOffset? EffectiveTo { get; set; } = effectiveTo;
    public string? ConfigurationJson { get; set; } = configurationJson;
}

public sealed class AuditEvent(
    Guid accountId,
    string actorType,
    string actorId,
    string action,
    string resourceType,
    string resourceId,
    string result,
    string? oldValuesJson,
    string? newValuesJson,
    string? reason,
    string? ipAddress,
    string? userAgent,
    string? correlationId) : BaseEntity
{
    public Guid AuditEventId { get; private set; } = Guid.NewGuid();
    public Guid AccountId { get; set; } = accountId;
    public string ActorType { get; set; } = actorType;
    public string ActorId { get; set; } = actorId;
    public string Action { get; set; } = action;
    public string ResourceType { get; set; } = resourceType;
    public string ResourceId { get; set; } = resourceId;
    public string Result { get; set; } = result;
    public string? OldValuesJson { get; set; } = oldValuesJson;
    public string? NewValuesJson { get; set; } = newValuesJson;
    public string? Reason { get; set; } = reason;
    public string? IpAddress { get; set; } = ipAddress;
    public string? UserAgent { get; set; } = userAgent;
    public string? CorrelationId { get; set; } = correlationId;
    public DateTimeOffset OccurredAt { get; private set; } = DateTimeOffset.UtcNow;
}

public sealed class Document(
    Guid accountId,
    string ownerEntityType,
    string ownerEntityId,
    string uploadedByPrincipalType,
    string uploadedByPrincipalId,
    string storageProvider,
    string storageKey,
    string contentType,
    long sizeBytes,
    string sha256Hash,
    string classification,
    string status,
    DateTimeOffset? expiresAt,
    string visibilityScope,
    string scanStatus) : BaseAuditableEntity
{
    public Guid DocumentId { get; private set; } = Guid.NewGuid();
    public Guid AccountId { get; set; } = accountId;
    public string OwnerEntityType { get; set; } = ownerEntityType;
    public string OwnerEntityId { get; set; } = ownerEntityId;
    public string UploadedByPrincipalType { get; set; } = uploadedByPrincipalType;
    public string UploadedByPrincipalId { get; set; } = uploadedByPrincipalId;
    public string StorageProvider { get; set; } = storageProvider;
    public string StorageKey { get; set; } = storageKey;
    public string ContentType { get; set; } = contentType;
    public long SizeBytes { get; set; } = sizeBytes;
    public string Sha256Hash { get; set; } = sha256Hash;
    public string Classification { get; set; } = classification;
    public string Status { get; set; } = status;
    public DateTimeOffset? ExpiresAt { get; set; } = expiresAt;
    public string VisibilityScope { get; set; } = visibilityScope;
    public string ScanStatus { get; set; } = scanStatus;
}

public sealed class NotificationRule(
    Guid accountId,
    string ruleKey,
    string ruleType,
    bool enabled,
    string triggerEvent,
    string recipientSelector,
    string channelsJson,
    string? throttlingJson,
    string? configurationJson) : BaseAuditableEntity
{
    public Guid NotificationRuleId { get; private set; } = Guid.NewGuid();
    public Guid AccountId { get; set; } = accountId;
    public string RuleKey { get; set; } = ruleKey;
    public string RuleType { get; set; } = ruleType;
    public bool Enabled { get; set; } = enabled;
    public string TriggerEvent { get; set; } = triggerEvent;
    public string RecipientSelector { get; set; } = recipientSelector;
    public string ChannelsJson { get; set; } = channelsJson;
    public string? ThrottlingJson { get; set; } = throttlingJson;
    public string? ConfigurationJson { get; set; } = configurationJson;
}

public sealed class AlertEvent(
    Guid accountId,
    string eventType,
    string severity,
    string sourceModule,
    string resourceType,
    string resourceId,
    string status,
    string? payloadJson,
    string deduplicationKey) : BaseAuditableEntity
{
    public Guid AlertEventId { get; private set; } = Guid.NewGuid();
    public Guid AccountId { get; set; } = accountId;
    public string EventType { get; set; } = eventType;
    public string Severity { get; set; } = severity;
    public string SourceModule { get; set; } = sourceModule;
    public string ResourceType { get; set; } = resourceType;
    public string ResourceId { get; set; } = resourceId;
    public string Status { get; set; } = status;
    public DateTimeOffset FirstSeenAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastSeenAt { get; set; } = DateTimeOffset.UtcNow;
    public string? PayloadJson { get; set; } = payloadJson;
    public string DeduplicationKey { get; set; } = deduplicationKey;
}

public sealed class NotificationDelivery(
    Guid accountId,
    Guid? notificationRuleId,
    Guid? alertEventId,
    string channel,
    string recipientPrincipalType,
    string recipient,
    string status) : BaseAuditableEntity
{
    public Guid NotificationDeliveryId { get; private set; } = Guid.NewGuid();
    public Guid AccountId { get; set; } = accountId;
    public Guid? NotificationRuleId { get; set; } = notificationRuleId;
    public Guid? AlertEventId { get; set; } = alertEventId;
    public string Channel { get; set; } = channel;
    public string RecipientPrincipalType { get; set; } = recipientPrincipalType;
    public string Recipient { get; set; } = recipient;
    public string Status { get; set; } = status;
    public int Attempts { get; set; }
    public string? ProviderMessageId { get; set; }
    public string? Error { get; set; }
    public DateTimeOffset? SentAt { get; set; }
    public DateTimeOffset? ReadAt { get; set; }
}

public sealed class BackgroundJobRun(
    string jobKey,
    Guid? accountId,
    string? resourceKey,
    string idempotencyKey,
    string status,
    int attempts,
    DateTimeOffset startedAt) : BaseEntity
{
    public Guid BackgroundJobRunId { get; private set; } = Guid.NewGuid();
    public string JobKey { get; set; } = jobKey;
    public Guid? AccountId { get; set; } = accountId;
    public string? ResourceKey { get; set; } = resourceKey;
    public string IdempotencyKey { get; set; } = idempotencyKey;
    public string Status { get; set; } = status;
    public int Attempts { get; set; } = attempts;
    public DateTimeOffset StartedAt { get; set; } = startedAt;
    public DateTimeOffset? CompletedAt { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
}

public sealed class PublicLinkGrant(
    Guid accountId,
    string resourceType,
    string resourceId,
    string scopes,
    string purpose,
    string subjectTokenIdHash,
    DateTimeOffset expiresAt,
    string createdBy) : BaseAuditableEntity
{
    public Guid PublicLinkGrantId { get; private set; } = Guid.NewGuid();
    public Guid AccountId { get; set; } = accountId;
    public string ResourceType { get; set; } = resourceType;
    public string ResourceId { get; set; } = resourceId;
    public string Scopes { get; set; } = scopes;
    public string Purpose { get; set; } = purpose;
    public string SubjectTokenIdHash { get; set; } = subjectTokenIdHash;
    public DateTimeOffset ExpiresAt { get; set; } = expiresAt;
    public DateTimeOffset? RevokedAt { get; set; }
    public string? RevokedBy { get; set; }
    public string CreatedByPrincipalId { get; set; } = createdBy;
    public int AccessCount { get; set; }
    public DateTimeOffset? LastAccessedAt { get; set; }
}

public sealed class AccountSupportGrant(
    Guid accountId,
    Guid supportUserId,
    string reason,
    string ticketReference,
    string accessLevel,
    DateTimeOffset startsAt,
    DateTimeOffset endsAt) : BaseAuditableEntity
{
    public Guid AccountSupportGrantId { get; private set; } = Guid.NewGuid();
    public Guid AccountId { get; set; } = accountId;
    public Guid SupportUserId { get; set; } = supportUserId;
    public string Reason { get; set; } = reason;
    public string TicketReference { get; set; } = ticketReference;
    public string? ApprovedBy { get; set; }
    public DateTimeOffset? ApprovedAt { get; set; }
    public string AccessLevel { get; set; } = accessLevel;
    public DateTimeOffset StartsAt { get; set; } = startsAt;
    public DateTimeOffset EndsAt { get; set; } = endsAt;
    public DateTimeOffset? RevokedAt { get; set; }
    public string? RevokedBy { get; set; }
}
