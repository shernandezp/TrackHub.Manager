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

namespace TrackHub.Manager.Domain.Records;

public readonly record struct DriverDto(Guid AccountId, string Name, string? Phone, string? DocumentType, string? DocumentNumber, bool Active, string? EmployeeCode, string? LicenseNumber, DateOnly? LicenseExpiresAt, Guid? DefaultTransporterId);

public readonly record struct AccountFeatureDto(Guid AccountId, string FeatureKey, bool Enabled, string Tier, string Source, DateTimeOffset? EffectiveFrom, DateTimeOffset? EffectiveTo, string? ConfigurationJson);

public readonly record struct AuditEventDto(Guid AccountId, string ActorType, string ActorId, string Action, string ResourceType, string ResourceId, string Result, string? OldValuesJson, string? NewValuesJson, string? Reason, string? IpAddress, string? UserAgent, string? CorrelationId);

public readonly record struct DocumentDto(Guid AccountId, string OwnerEntityType, string OwnerEntityId, string UploadedByPrincipalType, string UploadedByPrincipalId, string StorageProvider, string StorageKey, string ContentType, long SizeBytes, string Sha256Hash, string Classification, string Status, DateTimeOffset? ExpiresAt, string VisibilityScope, string ScanStatus);

public readonly record struct NotificationRuleDto(Guid AccountId, string RuleKey, string RuleType, bool Enabled, string TriggerEvent, string RecipientSelector, string ChannelsJson, string? ThrottlingJson, string? ConfigurationJson);

public readonly record struct AlertEventDto(Guid AccountId, string EventType, string Severity, string SourceModule, string ResourceType, string ResourceId, string Status, string? PayloadJson, string DeduplicationKey);

public readonly record struct NotificationDeliveryDto(Guid AccountId, Guid? NotificationRuleId, Guid? AlertEventId, string Channel, string RecipientPrincipalType, string Recipient, string Status);

public readonly record struct BackgroundJobRunDto(string JobKey, Guid? AccountId, string? ResourceKey, string IdempotencyKey, string Status, int Attempts, DateTimeOffset StartedAt, DateTimeOffset? CompletedAt, string? ErrorCode, string? ErrorMessage);

public readonly record struct PublicLinkGrantDto(Guid AccountId, string ResourceType, string ResourceId, string Scopes, string Purpose, string? SubjectTokenIdHash, DateTimeOffset ExpiresAt, string CreatedByPrincipalId);

public readonly record struct AccountSupportGrantDto(Guid AccountId, Guid SupportUserId, string Reason, string TicketReference, string AccessLevel, DateTimeOffset StartsAt, DateTimeOffset EndsAt);
