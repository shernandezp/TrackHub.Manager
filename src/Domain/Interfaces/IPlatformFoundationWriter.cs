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

using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;

public interface IPlatformFoundationWriter
{
    Task<DriverVm> CreateDriverAsync(DriverDto driver, CancellationToken cancellationToken);
    Task UpdateDriverAsync(Guid driverId, DriverDto driver, CancellationToken cancellationToken);
    Task DeactivateDriverAsync(Guid driverId, CancellationToken cancellationToken);
    Task<AccountFeatureVm> SetAccountFeatureAsync(AccountFeatureDto feature, CancellationToken cancellationToken);
    Task DisableAccountFeatureAsync(Guid accountFeatureId, CancellationToken cancellationToken);
    Task UpdateAccountFeatureConfigurationAsync(Guid accountFeatureId, string? configurationJson, CancellationToken cancellationToken);
    Task<AuditEventVm> CreateAuditEventAsync(AuditEventDto auditEvent, CancellationToken cancellationToken);
    Task<DocumentVm> CreateDocumentMetadataAsync(DocumentDto document, CancellationToken cancellationToken);
    Task MarkDocumentUploadedAsync(Guid documentId, string status, CancellationToken cancellationToken);
    Task MarkDocumentScanResultAsync(Guid documentId, string scanStatus, CancellationToken cancellationToken);
    Task ExpireDocumentAsync(Guid documentId, DateTimeOffset expiresAt, CancellationToken cancellationToken);
    Task DeleteDocumentReferenceAsync(Guid documentId, CancellationToken cancellationToken);
    Task<NotificationRuleVm> CreateNotificationRuleAsync(NotificationRuleDto notificationRule, CancellationToken cancellationToken);
    Task UpdateNotificationRuleAsync(Guid notificationRuleId, NotificationRuleDto notificationRule, CancellationToken cancellationToken);
    Task DisableNotificationRuleAsync(Guid notificationRuleId, CancellationToken cancellationToken);
    Task<AlertEventVm> RecordAlertEventAsync(AlertEventDto alertEvent, CancellationToken cancellationToken);
    Task AcknowledgeAlertEventAsync(Guid alertEventId, CancellationToken cancellationToken);
    Task ResolveAlertEventAsync(Guid alertEventId, CancellationToken cancellationToken);
    Task<NotificationDeliveryVm> CreateNotificationDeliveryAsync(NotificationDeliveryDto notificationDelivery, CancellationToken cancellationToken);
    Task<BackgroundJobRunVm> CreateBackgroundJobRunAsync(BackgroundJobRunDto backgroundJobRun, CancellationToken cancellationToken);
    Task<PublicLinkGrantVm> CreatePublicLinkGrantAsync(PublicLinkGrantDto publicLinkGrant, CancellationToken cancellationToken);
    Task RevokePublicLinkGrantAsync(Guid publicLinkGrantId, string revokedBy, CancellationToken cancellationToken);
    Task RecordPublicLinkAccessAsync(Guid publicLinkGrantId, CancellationToken cancellationToken);
    Task<AccountSupportGrantVm> CreateAccountSupportGrantAsync(AccountSupportGrantDto accountSupportGrant, CancellationToken cancellationToken);
    Task ApproveAccountSupportGrantAsync(Guid accountSupportGrantId, string approvedBy, CancellationToken cancellationToken);
    Task RevokeAccountSupportGrantAsync(Guid accountSupportGrantId, string revokedBy, CancellationToken cancellationToken);
}
