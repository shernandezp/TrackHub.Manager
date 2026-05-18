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

using TrackHub.Manager.Application.Foundation.Commands;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<DriverVm> CreateDriver([Service] ISender sender, CreateDriverCommand command) => await sender.Send(command);
    public async Task<bool> UpdateDriver([Service] ISender sender, UpdateDriverCommand command) { await sender.Send(command); return true; }
    public async Task<bool> DeactivateDriver([Service] ISender sender, DeactivateDriverCommand command) { await sender.Send(command); return true; }
    public async Task<AccountFeatureVm> SetAccountFeature([Service] ISender sender, SetAccountFeatureCommand command) => await sender.Send(command);
    public async Task<bool> DisableAccountFeature([Service] ISender sender, DisableAccountFeatureCommand command) { await sender.Send(command); return true; }
    public async Task<bool> UpdateAccountFeatureConfiguration([Service] ISender sender, UpdateAccountFeatureConfigurationCommand command) { await sender.Send(command); return true; }
    public async Task<AuditEventVm> CreateAuditEvent([Service] ISender sender, CreateAuditEventCommand command) => await sender.Send(command);
    public async Task<DocumentVm> CreateDocumentMetadata([Service] ISender sender, CreateDocumentMetadataCommand command) => await sender.Send(command);
    public async Task<bool> MarkDocumentUploaded([Service] ISender sender, MarkDocumentUploadedCommand command) { await sender.Send(command); return true; }
    public async Task<bool> MarkDocumentScanResult([Service] ISender sender, MarkDocumentScanResultCommand command) { await sender.Send(command); return true; }
    public async Task<bool> ExpireDocument([Service] ISender sender, ExpireDocumentCommand command) { await sender.Send(command); return true; }
    public async Task<bool> DeleteDocumentReference([Service] ISender sender, DeleteDocumentReferenceCommand command) { await sender.Send(command); return true; }
    public async Task<NotificationRuleVm> CreateNotificationRule([Service] ISender sender, CreateNotificationRuleCommand command) => await sender.Send(command);
    public async Task<bool> UpdateNotificationRule([Service] ISender sender, UpdateNotificationRuleCommand command) { await sender.Send(command); return true; }
    public async Task<bool> DisableNotificationRule([Service] ISender sender, DisableNotificationRuleCommand command) { await sender.Send(command); return true; }
    public async Task<AlertEventVm> RecordAlertEvent([Service] ISender sender, RecordAlertEventCommand command) => await sender.Send(command);
    public async Task<bool> AcknowledgeAlertEvent([Service] ISender sender, AcknowledgeAlertEventCommand command) { await sender.Send(command); return true; }
    public async Task<bool> ResolveAlertEvent([Service] ISender sender, ResolveAlertEventCommand command) { await sender.Send(command); return true; }
    public async Task<NotificationDeliveryVm> CreateNotificationDelivery([Service] ISender sender, CreateNotificationDeliveryCommand command) => await sender.Send(command);
    public async Task<BackgroundJobRunVm> CreateBackgroundJobRun([Service] ISender sender, CreateBackgroundJobRunCommand command) => await sender.Send(command);
    public async Task<PublicLinkGrantVm> CreatePublicLinkGrant([Service] ISender sender, CreatePublicLinkGrantCommand command) => await sender.Send(command);
    public async Task<bool> RevokePublicLinkGrant([Service] ISender sender, RevokePublicLinkGrantCommand command) { await sender.Send(command); return true; }
    public async Task<bool> RecordPublicLinkAccess([Service] ISender sender, RecordPublicLinkAccessCommand command) { await sender.Send(command); return true; }
    public async Task<AccountSupportGrantVm> CreateAccountSupportGrant([Service] ISender sender, CreateAccountSupportGrantCommand command) => await sender.Send(command);
    public async Task<bool> ApproveAccountSupportGrant([Service] ISender sender, ApproveAccountSupportGrantCommand command) { await sender.Send(command); return true; }
    public async Task<bool> RevokeAccountSupportGrant([Service] ISender sender, RevokeAccountSupportGrantCommand command) { await sender.Send(command); return true; }
}
