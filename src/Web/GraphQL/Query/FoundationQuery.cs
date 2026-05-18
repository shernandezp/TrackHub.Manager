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

using TrackHub.Manager.Application.Foundation.Queries;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<CurrentPrincipalVm> GetCurrentPrincipal([Service] ISender sender)
        => await sender.Send(new GetCurrentPrincipalQuery());

    public async Task<DriverVm> GetDriver([Service] ISender sender, [AsParameters] GetDriverQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<DriverVm>> GetDriversByAccount([Service] ISender sender, [AsParameters] GetDriversByAccountQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<DriverAssignmentVm>> GetDriverAssignments([Service] ISender sender, [AsParameters] GetDriverAssignmentsQuery query)
        => await sender.Send(query);

    public async Task<bool> ValidateDriverAssignment([Service] ISender sender, [AsParameters] ValidateDriverAssignmentQuery query)
        => await sender.Send(query);

    public async Task<bool> ValidateGroupVisibility([Service] ISender sender, [AsParameters] ValidateGroupVisibilityQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<AccountFeatureVm>> GetAccountFeatures([Service] ISender sender, [AsParameters] GetAccountFeaturesQuery query)
        => await sender.Send(query);

    public async Task<bool> ValidateFeatureEnabled([Service] ISender sender, [AsParameters] ValidateFeatureEnabledQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<AuditEventVm>> GetAuditTrail([Service] ISender sender, [AsParameters] GetAuditTrailQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<DocumentVm>> GetDocumentsForOwner([Service] ISender sender, [AsParameters] GetDocumentsForOwnerQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<NotificationRuleVm>> GetNotificationRules([Service] ISender sender, [AsParameters] GetNotificationRulesQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<AlertEventVm>> GetAlertEvents([Service] ISender sender, [AsParameters] GetAlertEventsQuery query)
        => await sender.Send(query);

    public async Task<PublicLinkGrantVm> GetPublicLinkGrant([Service] ISender sender, [AsParameters] GetPublicLinkGrantQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<PublicLinkGrantVm>> GetPublicLinkGrantsByAccount([Service] ISender sender, [AsParameters] GetPublicLinkGrantsByAccountQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<BackgroundJobRunVm>> GetBackgroundJobRuns([Service] ISender sender, [AsParameters] GetBackgroundJobRunsQuery query)
        => await sender.Send(query);

    public async Task<AccountSupportGrantVm> GetSupportGrantStatus([Service] ISender sender, [AsParameters] GetSupportGrantStatusQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<AccountSupportGrantVm>> GetAccountSupportGrants([Service] ISender sender, [AsParameters] GetAccountSupportGrantsQuery query)
        => await sender.Send(query);
}
