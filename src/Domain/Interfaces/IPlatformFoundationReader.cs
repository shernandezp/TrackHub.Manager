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

namespace TrackHub.Manager.Domain.Interfaces;

public interface IPlatformFoundationReader
{
    Task<DriverVm> GetDriverAsync(Guid driverId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DriverVm>> GetDriversByAccountAsync(Guid accountId, int skip, int take, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DriverAssignmentVm>> GetDriverAssignmentsAsync(Guid driverId, CancellationToken cancellationToken);
    Task<bool> ValidateDriverAssignmentAsync(Guid driverId, string resourceType, string resourceId, CancellationToken cancellationToken);
    Task<bool> ValidateGroupVisibilityAsync(Guid accountId, Guid userId, string resourceType, string resourceId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<AccountFeatureVm>> GetAccountFeaturesAsync(Guid accountId, CancellationToken cancellationToken);
    Task<bool> ValidateFeatureEnabledAsync(Guid accountId, string featureKey, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<AuditEventVm>> GetAuditTrailAsync(Guid accountId, DateTimeOffset? from, DateTimeOffset? to, int skip, int take, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DocumentVm>> GetDocumentsForOwnerAsync(Guid accountId, string ownerEntityType, string ownerEntityId, DateTimeOffset? from, DateTimeOffset? to, int skip, int take, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<NotificationRuleVm>> GetNotificationRulesAsync(Guid accountId, int skip, int take, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<AlertEventVm>> GetAlertEventsAsync(Guid accountId, DateTimeOffset? from, DateTimeOffset? to, int skip, int take, CancellationToken cancellationToken);
    Task<PublicLinkGrantVm> GetPublicLinkGrantAsync(Guid publicLinkGrantId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<PublicLinkGrantVm>> GetPublicLinkGrantsByAccountAsync(Guid accountId, int skip, int take, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<BackgroundJobRunVm>> GetBackgroundJobRunsAsync(Guid? accountId, DateTimeOffset? from, DateTimeOffset? to, int skip, int take, CancellationToken cancellationToken);
    Task<AccountSupportGrantVm> GetSupportGrantStatusAsync(Guid accountSupportGrantId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<AccountSupportGrantVm>> GetAccountSupportGrantsAsync(Guid? accountId, int skip, int take, CancellationToken cancellationToken);
}
