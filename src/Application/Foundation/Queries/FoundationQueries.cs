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

using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Foundation.Queries;

[Authorize(Resource = Resources.Profile, Action = Actions.Read)]
public readonly record struct GetCurrentPrincipalQuery() : IRequest<CurrentPrincipalVm>;

public readonly record struct CurrentPrincipalVm(string? SubjectId, PrincipalType PrincipalType, Guid? UserId, Guid? DriverId, string? ClientId, Guid? PublicLinkGrantId, string? Role, Guid? AccountId, IReadOnlyCollection<string> Scopes, IReadOnlyCollection<string> Audiences, string? CorrelationId);

public class GetCurrentPrincipalQueryHandler(ICurrentPrincipal principal) : IRequestHandler<GetCurrentPrincipalQuery, CurrentPrincipalVm>
{
    public Task<CurrentPrincipalVm> Handle(GetCurrentPrincipalQuery request, CancellationToken cancellationToken)
        => Task.FromResult(new CurrentPrincipalVm(principal.SubjectId, principal.PrincipalType, principal.UserId, principal.DriverId, principal.ClientId, principal.PublicLinkGrantId, principal.Role, principal.AccountId, principal.Scopes, principal.Audiences, principal.CorrelationId));
}

[Authorize(Resource = Resources.Drivers, Action = Actions.Read)]
public readonly record struct GetDriverQuery(Guid DriverId) : IRequest<DriverVm>;

public class GetDriverQueryHandler(IPlatformFoundationReader reader) : IRequestHandler<GetDriverQuery, DriverVm>
{
    public async Task<DriverVm> Handle(GetDriverQuery request, CancellationToken cancellationToken)
        => await reader.GetDriverAsync(request.DriverId, cancellationToken);
}

[Authorize(Resource = Resources.Drivers, Action = Actions.Read)]
public readonly record struct GetDriversByAccountQuery(Guid AccountId, int Skip = 0, int Take = 50) : IRequest<IReadOnlyCollection<DriverVm>>;

public class GetDriversByAccountQueryHandler(IPlatformFoundationReader reader) : IRequestHandler<GetDriversByAccountQuery, IReadOnlyCollection<DriverVm>>
{
    public async Task<IReadOnlyCollection<DriverVm>> Handle(GetDriversByAccountQuery request, CancellationToken cancellationToken)
        => await reader.GetDriversByAccountAsync(request.AccountId, request.Skip, request.Take, cancellationToken);
}

[Authorize(Resource = Resources.Drivers, Action = Actions.Read)]
public readonly record struct GetDriverAssignmentsQuery(Guid DriverId) : IRequest<IReadOnlyCollection<DriverAssignmentVm>>;

public class GetDriverAssignmentsQueryHandler(IPlatformFoundationReader reader) : IRequestHandler<GetDriverAssignmentsQuery, IReadOnlyCollection<DriverAssignmentVm>>
{
    public async Task<IReadOnlyCollection<DriverAssignmentVm>> Handle(GetDriverAssignmentsQuery request, CancellationToken cancellationToken)
        => await reader.GetDriverAssignmentsAsync(request.DriverId, cancellationToken);
}

[Authorize(Resource = Resources.Drivers, Action = Actions.Read)]
public readonly record struct ValidateDriverAssignmentQuery(Guid DriverId, string ResourceType, string ResourceId) : IRequest<bool>;

public class ValidateDriverAssignmentQueryHandler(IPlatformFoundationReader reader) : IRequestHandler<ValidateDriverAssignmentQuery, bool>
{
    public async Task<bool> Handle(ValidateDriverAssignmentQuery request, CancellationToken cancellationToken)
        => await reader.ValidateDriverAssignmentAsync(request.DriverId, request.ResourceType, request.ResourceId, cancellationToken);
}

[Authorize(Resource = Resources.Groups, Action = Actions.Read)]
public readonly record struct ValidateGroupVisibilityQuery(Guid AccountId, Guid UserId, string ResourceType, string ResourceId) : IRequest<bool>;

public class ValidateGroupVisibilityQueryHandler(IPlatformFoundationReader reader) : IRequestHandler<ValidateGroupVisibilityQuery, bool>
{
    public async Task<bool> Handle(ValidateGroupVisibilityQuery request, CancellationToken cancellationToken)
        => await reader.ValidateGroupVisibilityAsync(request.AccountId, request.UserId, request.ResourceType, request.ResourceId, cancellationToken);
}

[Authorize(Resource = Resources.AccountFeatures, Action = Actions.Read)]
public readonly record struct GetAccountFeaturesQuery(Guid AccountId) : IRequest<IReadOnlyCollection<AccountFeatureVm>>;

public class GetAccountFeaturesQueryHandler(IPlatformFoundationReader reader) : IRequestHandler<GetAccountFeaturesQuery, IReadOnlyCollection<AccountFeatureVm>>
{
    public async Task<IReadOnlyCollection<AccountFeatureVm>> Handle(GetAccountFeaturesQuery request, CancellationToken cancellationToken)
        => await reader.GetAccountFeaturesAsync(request.AccountId, cancellationToken);
}

[Authorize(Resource = Resources.AccountFeatures, Action = Actions.Read)]
public readonly record struct ValidateFeatureEnabledQuery(Guid AccountId, string FeatureKey) : IRequest<bool>;

public class ValidateFeatureEnabledQueryHandler(IPlatformFoundationReader reader) : IRequestHandler<ValidateFeatureEnabledQuery, bool>
{
    public async Task<bool> Handle(ValidateFeatureEnabledQuery request, CancellationToken cancellationToken)
        => await reader.ValidateFeatureEnabledAsync(request.AccountId, request.FeatureKey, cancellationToken);
}

[Authorize(Resource = Resources.Audit, Action = Actions.Read)]
public readonly record struct GetAuditTrailQuery(Guid AccountId, DateTimeOffset? From = null, DateTimeOffset? To = null, int Skip = 0, int Take = 50) : IRequest<IReadOnlyCollection<AuditEventVm>>;

public class GetAuditTrailQueryHandler(IPlatformFoundationReader reader) : IRequestHandler<GetAuditTrailQuery, IReadOnlyCollection<AuditEventVm>>
{
    public async Task<IReadOnlyCollection<AuditEventVm>> Handle(GetAuditTrailQuery request, CancellationToken cancellationToken)
        => await reader.GetAuditTrailAsync(request.AccountId, request.From, request.To, request.Skip, request.Take, cancellationToken);
}

[Authorize(Resource = Resources.Documents, Action = Actions.Read)]
public readonly record struct GetDocumentsForOwnerQuery(Guid AccountId, string OwnerEntityType, string OwnerEntityId, DateTimeOffset? From = null, DateTimeOffset? To = null, int Skip = 0, int Take = 50) : IRequest<IReadOnlyCollection<DocumentVm>>;

public class GetDocumentsForOwnerQueryHandler(IPlatformFoundationReader reader) : IRequestHandler<GetDocumentsForOwnerQuery, IReadOnlyCollection<DocumentVm>>
{
    public async Task<IReadOnlyCollection<DocumentVm>> Handle(GetDocumentsForOwnerQuery request, CancellationToken cancellationToken)
        => await reader.GetDocumentsForOwnerAsync(request.AccountId, request.OwnerEntityType, request.OwnerEntityId, request.From, request.To, request.Skip, request.Take, cancellationToken);
}

[Authorize(Resource = Resources.Notifications, Action = Actions.Read)]
public readonly record struct GetNotificationRulesQuery(Guid AccountId, int Skip = 0, int Take = 50) : IRequest<IReadOnlyCollection<NotificationRuleVm>>;

public class GetNotificationRulesQueryHandler(IPlatformFoundationReader reader) : IRequestHandler<GetNotificationRulesQuery, IReadOnlyCollection<NotificationRuleVm>>
{
    public async Task<IReadOnlyCollection<NotificationRuleVm>> Handle(GetNotificationRulesQuery request, CancellationToken cancellationToken)
        => await reader.GetNotificationRulesAsync(request.AccountId, request.Skip, request.Take, cancellationToken);
}

[Authorize(Resource = Resources.Alerts, Action = Actions.Read)]
public readonly record struct GetAlertEventsQuery(Guid AccountId, DateTimeOffset? From = null, DateTimeOffset? To = null, int Skip = 0, int Take = 50) : IRequest<IReadOnlyCollection<AlertEventVm>>;

public class GetAlertEventsQueryHandler(IPlatformFoundationReader reader) : IRequestHandler<GetAlertEventsQuery, IReadOnlyCollection<AlertEventVm>>
{
    public async Task<IReadOnlyCollection<AlertEventVm>> Handle(GetAlertEventsQuery request, CancellationToken cancellationToken)
        => await reader.GetAlertEventsAsync(request.AccountId, request.From, request.To, request.Skip, request.Take, cancellationToken);
}

[Authorize(Resource = Resources.PublicLinks, Action = Actions.Read)]
public readonly record struct GetPublicLinkGrantQuery(Guid PublicLinkGrantId) : IRequest<PublicLinkGrantVm>;

public class GetPublicLinkGrantQueryHandler(IPlatformFoundationReader reader) : IRequestHandler<GetPublicLinkGrantQuery, PublicLinkGrantVm>
{
    public async Task<PublicLinkGrantVm> Handle(GetPublicLinkGrantQuery request, CancellationToken cancellationToken)
        => await reader.GetPublicLinkGrantAsync(request.PublicLinkGrantId, cancellationToken);
}

[Authorize(Resource = Resources.PublicLinks, Action = Actions.Read)]
public readonly record struct GetPublicLinkGrantsByAccountQuery(Guid AccountId, int Skip = 0, int Take = 50) : IRequest<IReadOnlyCollection<PublicLinkGrantVm>>;

public class GetPublicLinkGrantsByAccountQueryHandler(IPlatformFoundationReader reader) : IRequestHandler<GetPublicLinkGrantsByAccountQuery, IReadOnlyCollection<PublicLinkGrantVm>>
{
    public async Task<IReadOnlyCollection<PublicLinkGrantVm>> Handle(GetPublicLinkGrantsByAccountQuery request, CancellationToken cancellationToken)
        => await reader.GetPublicLinkGrantsByAccountAsync(request.AccountId, request.Skip, request.Take, cancellationToken);
}

[Authorize(Resource = Resources.BackgroundJobs, Action = Actions.Read)]
public readonly record struct GetBackgroundJobRunsQuery(Guid? AccountId, DateTimeOffset? From = null, DateTimeOffset? To = null, int Skip = 0, int Take = 50) : IRequest<IReadOnlyCollection<BackgroundJobRunVm>>;

public class GetBackgroundJobRunsQueryHandler(IPlatformFoundationReader reader) : IRequestHandler<GetBackgroundJobRunsQuery, IReadOnlyCollection<BackgroundJobRunVm>>
{
    public async Task<IReadOnlyCollection<BackgroundJobRunVm>> Handle(GetBackgroundJobRunsQuery request, CancellationToken cancellationToken)
        => await reader.GetBackgroundJobRunsAsync(request.AccountId, request.From, request.To, request.Skip, request.Take, cancellationToken);
}

[Authorize(Resource = Resources.SupportGrants, Action = Actions.Read)]
public readonly record struct GetSupportGrantStatusQuery(Guid AccountSupportGrantId) : IRequest<AccountSupportGrantVm>;

public class GetSupportGrantStatusQueryHandler(IPlatformFoundationReader reader) : IRequestHandler<GetSupportGrantStatusQuery, AccountSupportGrantVm>
{
    public async Task<AccountSupportGrantVm> Handle(GetSupportGrantStatusQuery request, CancellationToken cancellationToken)
        => await reader.GetSupportGrantStatusAsync(request.AccountSupportGrantId, cancellationToken);
}

[Authorize(Resource = Resources.SupportGrants, Action = Actions.Read)]
public readonly record struct GetAccountSupportGrantsQuery(Guid? AccountId = null, int Skip = 0, int Take = 50) : IRequest<IReadOnlyCollection<AccountSupportGrantVm>>;

public class GetAccountSupportGrantsQueryHandler(IPlatformFoundationReader reader) : IRequestHandler<GetAccountSupportGrantsQuery, IReadOnlyCollection<AccountSupportGrantVm>>
{
    public async Task<IReadOnlyCollection<AccountSupportGrantVm>> Handle(GetAccountSupportGrantsQuery request, CancellationToken cancellationToken)
        => await reader.GetAccountSupportGrantsAsync(request.AccountId, request.Skip, request.Take, cancellationToken);
}
