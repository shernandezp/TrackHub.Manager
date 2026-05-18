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

namespace TrackHub.Manager.Application.Foundation.Commands;

[Authorize(Resource = Resources.Drivers, Action = Actions.Write)]
public readonly record struct CreateDriverCommand(DriverDto Driver) : IRequest<DriverVm>;
public class CreateDriverCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<CreateDriverCommand, DriverVm>
{
    public async Task<DriverVm> Handle(CreateDriverCommand request, CancellationToken cancellationToken) => await writer.CreateDriverAsync(request.Driver, cancellationToken);
}

[Authorize(Resource = Resources.Drivers, Action = Actions.Edit)]
public readonly record struct UpdateDriverCommand(Guid DriverId, DriverDto Driver) : IRequest;
public class UpdateDriverCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<UpdateDriverCommand>
{
    public async Task Handle(UpdateDriverCommand request, CancellationToken cancellationToken) => await writer.UpdateDriverAsync(request.DriverId, request.Driver, cancellationToken);
}

[Authorize(Resource = Resources.Drivers, Action = Actions.Delete)]
public readonly record struct DeactivateDriverCommand(Guid DriverId) : IRequest;
public class DeactivateDriverCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<DeactivateDriverCommand>
{
    public async Task Handle(DeactivateDriverCommand request, CancellationToken cancellationToken) => await writer.DeactivateDriverAsync(request.DriverId, cancellationToken);
}

[Authorize(Resource = Resources.AccountFeatures, Action = Actions.Write)]
public readonly record struct SetAccountFeatureCommand(AccountFeatureDto Feature) : IRequest<AccountFeatureVm>;
public class SetAccountFeatureCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<SetAccountFeatureCommand, AccountFeatureVm>
{
    public async Task<AccountFeatureVm> Handle(SetAccountFeatureCommand request, CancellationToken cancellationToken) => await writer.SetAccountFeatureAsync(request.Feature, cancellationToken);
}

[Authorize(Resource = Resources.AccountFeatures, Action = Actions.Delete)]
public readonly record struct DisableAccountFeatureCommand(Guid AccountFeatureId) : IRequest;
public class DisableAccountFeatureCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<DisableAccountFeatureCommand>
{
    public async Task Handle(DisableAccountFeatureCommand request, CancellationToken cancellationToken) => await writer.DisableAccountFeatureAsync(request.AccountFeatureId, cancellationToken);
}

[Authorize(Resource = Resources.AccountFeatures, Action = Actions.Edit)]
public readonly record struct UpdateAccountFeatureConfigurationCommand(Guid AccountFeatureId, string? ConfigurationJson) : IRequest;
public class UpdateAccountFeatureConfigurationCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<UpdateAccountFeatureConfigurationCommand>
{
    public async Task Handle(UpdateAccountFeatureConfigurationCommand request, CancellationToken cancellationToken) => await writer.UpdateAccountFeatureConfigurationAsync(request.AccountFeatureId, request.ConfigurationJson, cancellationToken);
}

[Authorize(Resource = Resources.Audit, Action = Actions.Write)]
public readonly record struct CreateAuditEventCommand(AuditEventDto AuditEvent) : IRequest<AuditEventVm>;
public class CreateAuditEventCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<CreateAuditEventCommand, AuditEventVm>
{
    public async Task<AuditEventVm> Handle(CreateAuditEventCommand request, CancellationToken cancellationToken) => await writer.CreateAuditEventAsync(request.AuditEvent, cancellationToken);
}

[Authorize(Resource = Resources.Documents, Action = Actions.Write)]
public readonly record struct CreateDocumentMetadataCommand(DocumentDto Document) : IRequest<DocumentVm>;
public class CreateDocumentMetadataCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<CreateDocumentMetadataCommand, DocumentVm>
{
    public async Task<DocumentVm> Handle(CreateDocumentMetadataCommand request, CancellationToken cancellationToken) => await writer.CreateDocumentMetadataAsync(request.Document, cancellationToken);
}

[Authorize(Resource = Resources.Documents, Action = Actions.Edit)]
public readonly record struct MarkDocumentUploadedCommand(Guid DocumentId, string Status) : IRequest;
public class MarkDocumentUploadedCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<MarkDocumentUploadedCommand>
{
    public async Task Handle(MarkDocumentUploadedCommand request, CancellationToken cancellationToken) => await writer.MarkDocumentUploadedAsync(request.DocumentId, request.Status, cancellationToken);
}

[Authorize(Resource = Resources.Documents, Action = Actions.Edit)]
public readonly record struct MarkDocumentScanResultCommand(Guid DocumentId, string ScanStatus) : IRequest;
public class MarkDocumentScanResultCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<MarkDocumentScanResultCommand>
{
    public async Task Handle(MarkDocumentScanResultCommand request, CancellationToken cancellationToken) => await writer.MarkDocumentScanResultAsync(request.DocumentId, request.ScanStatus, cancellationToken);
}

[Authorize(Resource = Resources.Documents, Action = Actions.Edit)]
public readonly record struct ExpireDocumentCommand(Guid DocumentId, DateTimeOffset ExpiresAt) : IRequest;
public class ExpireDocumentCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<ExpireDocumentCommand>
{
    public async Task Handle(ExpireDocumentCommand request, CancellationToken cancellationToken) => await writer.ExpireDocumentAsync(request.DocumentId, request.ExpiresAt, cancellationToken);
}

[Authorize(Resource = Resources.Documents, Action = Actions.Delete)]
public readonly record struct DeleteDocumentReferenceCommand(Guid DocumentId) : IRequest;
public class DeleteDocumentReferenceCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<DeleteDocumentReferenceCommand>
{
    public async Task Handle(DeleteDocumentReferenceCommand request, CancellationToken cancellationToken) => await writer.DeleteDocumentReferenceAsync(request.DocumentId, cancellationToken);
}

[Authorize(Resource = Resources.Notifications, Action = Actions.Write)]
public readonly record struct CreateNotificationRuleCommand(NotificationRuleDto NotificationRule) : IRequest<NotificationRuleVm>;
public class CreateNotificationRuleCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<CreateNotificationRuleCommand, NotificationRuleVm>
{
    public async Task<NotificationRuleVm> Handle(CreateNotificationRuleCommand request, CancellationToken cancellationToken) => await writer.CreateNotificationRuleAsync(request.NotificationRule, cancellationToken);
}

[Authorize(Resource = Resources.Notifications, Action = Actions.Edit)]
public readonly record struct UpdateNotificationRuleCommand(Guid NotificationRuleId, NotificationRuleDto NotificationRule) : IRequest;
public class UpdateNotificationRuleCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<UpdateNotificationRuleCommand>
{
    public async Task Handle(UpdateNotificationRuleCommand request, CancellationToken cancellationToken) => await writer.UpdateNotificationRuleAsync(request.NotificationRuleId, request.NotificationRule, cancellationToken);
}

[Authorize(Resource = Resources.Notifications, Action = Actions.Delete)]
public readonly record struct DisableNotificationRuleCommand(Guid NotificationRuleId) : IRequest;
public class DisableNotificationRuleCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<DisableNotificationRuleCommand>
{
    public async Task Handle(DisableNotificationRuleCommand request, CancellationToken cancellationToken) => await writer.DisableNotificationRuleAsync(request.NotificationRuleId, cancellationToken);
}

[Authorize(Resource = Resources.Alerts, Action = Actions.Write)]
public readonly record struct RecordAlertEventCommand(AlertEventDto AlertEvent) : IRequest<AlertEventVm>;
public class RecordAlertEventCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<RecordAlertEventCommand, AlertEventVm>
{
    public async Task<AlertEventVm> Handle(RecordAlertEventCommand request, CancellationToken cancellationToken) => await writer.RecordAlertEventAsync(request.AlertEvent, cancellationToken);
}

[Authorize(Resource = Resources.Alerts, Action = Actions.Edit)]
public readonly record struct AcknowledgeAlertEventCommand(Guid AlertEventId) : IRequest;
public class AcknowledgeAlertEventCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<AcknowledgeAlertEventCommand>
{
    public async Task Handle(AcknowledgeAlertEventCommand request, CancellationToken cancellationToken) => await writer.AcknowledgeAlertEventAsync(request.AlertEventId, cancellationToken);
}

[Authorize(Resource = Resources.Alerts, Action = Actions.Edit)]
public readonly record struct ResolveAlertEventCommand(Guid AlertEventId) : IRequest;
public class ResolveAlertEventCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<ResolveAlertEventCommand>
{
    public async Task Handle(ResolveAlertEventCommand request, CancellationToken cancellationToken) => await writer.ResolveAlertEventAsync(request.AlertEventId, cancellationToken);
}

[Authorize(Resource = Resources.Notifications, Action = Actions.Write)]
public readonly record struct CreateNotificationDeliveryCommand(NotificationDeliveryDto NotificationDelivery) : IRequest<NotificationDeliveryVm>;
public class CreateNotificationDeliveryCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<CreateNotificationDeliveryCommand, NotificationDeliveryVm>
{
    public async Task<NotificationDeliveryVm> Handle(CreateNotificationDeliveryCommand request, CancellationToken cancellationToken) => await writer.CreateNotificationDeliveryAsync(request.NotificationDelivery, cancellationToken);
}

[Authorize(Resource = Resources.BackgroundJobs, Action = Actions.Write)]
public readonly record struct CreateBackgroundJobRunCommand(BackgroundJobRunDto BackgroundJobRun) : IRequest<BackgroundJobRunVm>;
public class CreateBackgroundJobRunCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<CreateBackgroundJobRunCommand, BackgroundJobRunVm>
{
    public async Task<BackgroundJobRunVm> Handle(CreateBackgroundJobRunCommand request, CancellationToken cancellationToken) => await writer.CreateBackgroundJobRunAsync(request.BackgroundJobRun, cancellationToken);
}

[Authorize(Resource = Resources.PublicLinks, Action = Actions.Write)]
public readonly record struct CreatePublicLinkGrantCommand(PublicLinkGrantDto PublicLinkGrant) : IRequest<PublicLinkGrantVm>;
public class CreatePublicLinkGrantCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<CreatePublicLinkGrantCommand, PublicLinkGrantVm>
{
    public async Task<PublicLinkGrantVm> Handle(CreatePublicLinkGrantCommand request, CancellationToken cancellationToken) => await writer.CreatePublicLinkGrantAsync(request.PublicLinkGrant, cancellationToken);
}

[Authorize(Resource = Resources.PublicLinks, Action = Actions.Delete)]
public readonly record struct RevokePublicLinkGrantCommand(Guid PublicLinkGrantId, string RevokedBy) : IRequest;
public class RevokePublicLinkGrantCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<RevokePublicLinkGrantCommand>
{
    public async Task Handle(RevokePublicLinkGrantCommand request, CancellationToken cancellationToken) => await writer.RevokePublicLinkGrantAsync(request.PublicLinkGrantId, request.RevokedBy, cancellationToken);
}

[Authorize(Resource = Resources.PublicLinks, Action = Actions.Read)]
public readonly record struct RecordPublicLinkAccessCommand(Guid PublicLinkGrantId) : IRequest;
public class RecordPublicLinkAccessCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<RecordPublicLinkAccessCommand>
{
    public async Task Handle(RecordPublicLinkAccessCommand request, CancellationToken cancellationToken) => await writer.RecordPublicLinkAccessAsync(request.PublicLinkGrantId, cancellationToken);
}

[Authorize(Resource = Resources.SupportGrants, Action = Actions.Write)]
public readonly record struct CreateAccountSupportGrantCommand(AccountSupportGrantDto AccountSupportGrant) : IRequest<AccountSupportGrantVm>;
public class CreateAccountSupportGrantCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<CreateAccountSupportGrantCommand, AccountSupportGrantVm>
{
    public async Task<AccountSupportGrantVm> Handle(CreateAccountSupportGrantCommand request, CancellationToken cancellationToken) => await writer.CreateAccountSupportGrantAsync(request.AccountSupportGrant, cancellationToken);
}

[Authorize(Resource = Resources.SupportGrants, Action = Actions.Edit)]
public readonly record struct ApproveAccountSupportGrantCommand(Guid AccountSupportGrantId, string ApprovedBy) : IRequest;
public class ApproveAccountSupportGrantCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<ApproveAccountSupportGrantCommand>
{
    public async Task Handle(ApproveAccountSupportGrantCommand request, CancellationToken cancellationToken) => await writer.ApproveAccountSupportGrantAsync(request.AccountSupportGrantId, request.ApprovedBy, cancellationToken);
}

[Authorize(Resource = Resources.SupportGrants, Action = Actions.Delete)]
public readonly record struct RevokeAccountSupportGrantCommand(Guid AccountSupportGrantId, string RevokedBy) : IRequest;
public class RevokeAccountSupportGrantCommandHandler(IPlatformFoundationWriter writer) : IRequestHandler<RevokeAccountSupportGrantCommand>
{
    public async Task Handle(RevokeAccountSupportGrantCommand request, CancellationToken cancellationToken) => await writer.RevokeAccountSupportGrantAsync(request.AccountSupportGrantId, request.RevokedBy, cancellationToken);
}
