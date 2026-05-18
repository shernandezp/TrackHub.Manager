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

namespace TrackHub.Manager.Domain.Models;

public readonly record struct DriverVm(Guid DriverId, Guid AccountId, string Name, string? Phone, string? DocumentType, string? DocumentNumber, bool Active, string? EmployeeCode, string? LicenseNumber, DateOnly? LicenseExpiresAt, Guid? DefaultTransporterId, DateTimeOffset LastModified);

public readonly record struct DriverAssignmentVm(Guid DriverId, Guid AccountId, string ResourceType, string ResourceId, bool Active);

public readonly record struct AccountFeatureVm(Guid AccountFeatureId, Guid AccountId, string FeatureKey, bool Enabled, string Tier, string Source, DateTimeOffset? EffectiveFrom, DateTimeOffset? EffectiveTo, string? ConfigurationJson, DateTimeOffset LastModified);

public readonly record struct AuditEventVm(Guid AuditEventId, Guid AccountId, string ActorType, string ActorId, string Action, string ResourceType, string ResourceId, string Result, string? Reason, string? IpAddress, string? UserAgent, string? CorrelationId, DateTimeOffset OccurredAt);

public readonly record struct DocumentVm(Guid DocumentId, Guid AccountId, string OwnerEntityType, string OwnerEntityId, string UploadedByPrincipalType, string UploadedByPrincipalId, string StorageProvider, string ContentType, long SizeBytes, string Sha256Hash, string Classification, string Status, DateTimeOffset? ExpiresAt, string VisibilityScope, string ScanStatus, DateTimeOffset LastModified);

public readonly record struct NotificationRuleVm(Guid NotificationRuleId, Guid AccountId, string RuleKey, string RuleType, bool Enabled, string TriggerEvent, string RecipientSelector, string ChannelsJson, string? ThrottlingJson, string? ConfigurationJson, DateTimeOffset LastModified);

public readonly record struct AlertEventVm(Guid AlertEventId, Guid AccountId, string EventType, string Severity, string SourceModule, string ResourceType, string ResourceId, string Status, DateTimeOffset FirstSeenAt, DateTimeOffset LastSeenAt, string? PayloadJson, string DeduplicationKey, DateTimeOffset LastModified);

public readonly record struct NotificationDeliveryVm(Guid NotificationDeliveryId, Guid AccountId, Guid? NotificationRuleId, Guid? AlertEventId, string Channel, string RecipientPrincipalType, string Recipient, string Status, int Attempts, string? ProviderMessageId, string? Error, DateTimeOffset? SentAt, DateTimeOffset? ReadAt, DateTimeOffset LastModified);

public readonly record struct BackgroundJobRunVm(Guid BackgroundJobRunId, string JobKey, Guid? AccountId, string? ResourceKey, string IdempotencyKey, string Status, int Attempts, DateTimeOffset StartedAt, DateTimeOffset? CompletedAt, string? ErrorCode, string? ErrorMessage);

public readonly record struct PublicLinkGrantVm(Guid PublicLinkGrantId, Guid AccountId, string ResourceType, string ResourceId, string Scopes, string Purpose, DateTimeOffset ExpiresAt, DateTimeOffset? RevokedAt, string? RevokedBy, string CreatedByPrincipalId, int AccessCount, DateTimeOffset? LastAccessedAt, DateTimeOffset LastModified, string? Token);

public readonly record struct AccountSupportGrantVm(Guid AccountSupportGrantId, Guid AccountId, Guid SupportUserId, string Reason, string TicketReference, string? ApprovedBy, DateTimeOffset? ApprovedAt, string AccessLevel, DateTimeOffset StartsAt, DateTimeOffset EndsAt, DateTimeOffset? RevokedAt, string? RevokedBy, DateTimeOffset LastModified);
