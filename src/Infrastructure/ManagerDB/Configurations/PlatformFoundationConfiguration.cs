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

using Common.Domain.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackHub.Manager.Infrastructure.Entities;

namespace TrackHub.Manager.Infrastructure.Configurations;

public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.ToTable(name: TableMetadata.Driver, schema: SchemaMetadata.Application);
        builder.Property(x => x.DriverId).HasColumnName("id");
        builder.Property(x => x.AccountId).HasColumnName("accountid");
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.Phone).HasColumnName("phone").HasMaxLength(ColumnMetadata.DefaultPhoneNumberLength);
        builder.Property(x => x.DocumentType).HasColumnName("documenttype").HasMaxLength(ColumnMetadata.DefaultFieldLength);
        builder.Property(x => x.DocumentNumber).HasColumnName("documentnumber").HasMaxLength(ColumnMetadata.DefaultNameLength);
        builder.Property(x => x.Active).HasColumnName("active");
        builder.Property(x => x.EmployeeCode).HasColumnName("employeecode").HasMaxLength(ColumnMetadata.DefaultNameLength);
        builder.Property(x => x.LicenseNumber).HasColumnName("licensenumber").HasMaxLength(ColumnMetadata.DefaultNameLength);
        builder.Property(x => x.LicenseExpiresAt).HasColumnName("licenseexpiresat");
        builder.Property(x => x.DefaultTransporterId).HasColumnName("defaulttransporterid");
        builder.HasIndex(x => new { x.AccountId, x.DocumentNumber });
    }
}

public class AccountFeatureConfiguration : IEntityTypeConfiguration<AccountFeature>
{
    public void Configure(EntityTypeBuilder<AccountFeature> builder)
    {
        builder.ToTable(name: TableMetadata.AccountFeature, schema: SchemaMetadata.Application);
        builder.Property(x => x.AccountFeatureId).HasColumnName("id");
        builder.Property(x => x.AccountId).HasColumnName("accountid");
        builder.Property(x => x.FeatureKey).HasColumnName("featurekey").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.Enabled).HasColumnName("enabled");
        builder.Property(x => x.Tier).HasColumnName("tier").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.Source).HasColumnName("source").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.EffectiveFrom).HasColumnName("effectivefrom");
        builder.Property(x => x.EffectiveTo).HasColumnName("effectiveto");
        builder.Property(x => x.ConfigurationJson).HasColumnName("configurationjson").HasColumnType(ColumnMetadata.TextField);
        builder.HasIndex(x => new { x.AccountId, x.FeatureKey }).IsUnique();
    }
}

public class AuditEventConfiguration : IEntityTypeConfiguration<AuditEvent>
{
    public void Configure(EntityTypeBuilder<AuditEvent> builder)
    {
        builder.ToTable(name: TableMetadata.AuditEvent, schema: SchemaMetadata.Application);
        builder.Property(x => x.AuditEventId).HasColumnName("id");
        builder.Property(x => x.AccountId).HasColumnName("accountid");
        builder.Property(x => x.ActorType).HasColumnName("actortype").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.ActorId).HasColumnName("actorid").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.Action).HasColumnName("action").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.ResourceType).HasColumnName("resourcetype").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.ResourceId).HasColumnName("resourceid").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.Result).HasColumnName("result").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.OldValuesJson).HasColumnName("oldvaluesjson").HasColumnType(ColumnMetadata.TextField);
        builder.Property(x => x.NewValuesJson).HasColumnName("newvaluesjson").HasColumnType(ColumnMetadata.TextField);
        builder.Property(x => x.Reason).HasColumnName("reason").HasMaxLength(ColumnMetadata.DefaultDescriptionLength);
        builder.Property(x => x.IpAddress).HasColumnName("ipaddress").HasMaxLength(ColumnMetadata.DefaultNameLength);
        builder.Property(x => x.UserAgent).HasColumnName("useragent").HasMaxLength(ColumnMetadata.DefaultDescriptionLength);
        builder.Property(x => x.CorrelationId).HasColumnName("correlationid").HasMaxLength(ColumnMetadata.DefaultNameLength);
        builder.Property(x => x.OccurredAt).HasColumnName("occurredat");
        builder.HasIndex(x => new { x.AccountId, x.OccurredAt });
        builder.HasIndex(x => x.CorrelationId);
    }
}

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable(name: TableMetadata.Document, schema: SchemaMetadata.Application);
        builder.Property(x => x.DocumentId).HasColumnName("id");
        builder.Property(x => x.AccountId).HasColumnName("accountid");
        builder.Property(x => x.OwnerEntityType).HasColumnName("ownerentitytype").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.OwnerEntityId).HasColumnName("ownerentityid").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.UploadedByPrincipalType).HasColumnName("uploadedbyprincipaltype").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.UploadedByPrincipalId).HasColumnName("uploadedbyprincipalid").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.StorageProvider).HasColumnName("storageprovider").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.StorageKey).HasColumnName("storagekey").HasColumnType(ColumnMetadata.TextField).IsRequired();
        builder.Property(x => x.ContentType).HasColumnName("contenttype").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.SizeBytes).HasColumnName("sizebytes");
        builder.Property(x => x.Sha256Hash).HasColumnName("sha256hash").HasMaxLength(ColumnMetadata.DefaultTokenLength).IsRequired();
        builder.Property(x => x.Classification).HasColumnName("classification").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.ExpiresAt).HasColumnName("expiresat");
        builder.Property(x => x.VisibilityScope).HasColumnName("visibilityscope").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.ScanStatus).HasColumnName("scanstatus").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.HasIndex(x => new { x.AccountId, x.OwnerEntityType, x.OwnerEntityId });
        builder.HasIndex(x => x.Sha256Hash);
    }
}

public class NotificationRuleConfiguration : IEntityTypeConfiguration<NotificationRule>
{
    public void Configure(EntityTypeBuilder<NotificationRule> builder)
    {
        builder.ToTable(name: TableMetadata.NotificationRule, schema: SchemaMetadata.Application);
        builder.Property(x => x.NotificationRuleId).HasColumnName("id");
        builder.Property(x => x.AccountId).HasColumnName("accountid");
        builder.Property(x => x.RuleKey).HasColumnName("rulekey").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.RuleType).HasColumnName("ruletype").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.Enabled).HasColumnName("enabled");
        builder.Property(x => x.TriggerEvent).HasColumnName("triggerevent").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.RecipientSelector).HasColumnName("recipientselector").HasColumnType(ColumnMetadata.TextField).IsRequired();
        builder.Property(x => x.ChannelsJson).HasColumnName("channelsjson").HasColumnType(ColumnMetadata.TextField).IsRequired();
        builder.Property(x => x.ThrottlingJson).HasColumnName("throttlingjson").HasColumnType(ColumnMetadata.TextField);
        builder.Property(x => x.ConfigurationJson).HasColumnName("configurationjson").HasColumnType(ColumnMetadata.TextField);
        builder.HasIndex(x => new { x.AccountId, x.RuleKey }).IsUnique();
    }
}

public class AlertEventConfiguration : IEntityTypeConfiguration<AlertEvent>
{
    public void Configure(EntityTypeBuilder<AlertEvent> builder)
    {
        builder.ToTable(name: TableMetadata.AlertEvent, schema: SchemaMetadata.Application);
        builder.Property(x => x.AlertEventId).HasColumnName("id");
        builder.Property(x => x.AccountId).HasColumnName("accountid");
        builder.Property(x => x.EventType).HasColumnName("eventtype").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.Severity).HasColumnName("severity").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.SourceModule).HasColumnName("sourcemodule").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.ResourceType).HasColumnName("resourcetype").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.ResourceId).HasColumnName("resourceid").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.FirstSeenAt).HasColumnName("firstseenat");
        builder.Property(x => x.LastSeenAt).HasColumnName("lastseenat");
        builder.Property(x => x.PayloadJson).HasColumnName("payloadjson").HasColumnType(ColumnMetadata.TextField);
        builder.Property(x => x.DeduplicationKey).HasColumnName("deduplicationkey").HasMaxLength(ColumnMetadata.DefaultTokenLength).IsRequired();
        builder.HasIndex(x => new { x.AccountId, x.Status, x.LastSeenAt });
        builder.HasIndex(x => x.DeduplicationKey);
    }
}

public class NotificationDeliveryConfiguration : IEntityTypeConfiguration<NotificationDelivery>
{
    public void Configure(EntityTypeBuilder<NotificationDelivery> builder)
    {
        builder.ToTable(name: TableMetadata.NotificationDelivery, schema: SchemaMetadata.Application);
        builder.Property(x => x.NotificationDeliveryId).HasColumnName("id");
        builder.Property(x => x.AccountId).HasColumnName("accountid");
        builder.Property(x => x.NotificationRuleId).HasColumnName("notificationruleid");
        builder.Property(x => x.AlertEventId).HasColumnName("alerteventid");
        builder.Property(x => x.Channel).HasColumnName("channel").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.RecipientPrincipalType).HasColumnName("recipientprincipaltype").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.Recipient).HasColumnName("recipient").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.Attempts).HasColumnName("attempts");
        builder.Property(x => x.ProviderMessageId).HasColumnName("providermessageid").HasMaxLength(ColumnMetadata.DefaultNameLength);
        builder.Property(x => x.Error).HasColumnName("error").HasColumnType(ColumnMetadata.TextField);
        builder.Property(x => x.SentAt).HasColumnName("sentat");
        builder.Property(x => x.ReadAt).HasColumnName("readat");
        builder.HasIndex(x => new { x.AccountId, x.Status, x.Channel });
    }
}

public class BackgroundJobRunConfiguration : IEntityTypeConfiguration<BackgroundJobRun>
{
    public void Configure(EntityTypeBuilder<BackgroundJobRun> builder)
    {
        builder.ToTable(name: TableMetadata.BackgroundJobRun, schema: SchemaMetadata.Application);
        builder.Property(x => x.BackgroundJobRunId).HasColumnName("id");
        builder.Property(x => x.JobKey).HasColumnName("jobkey").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.AccountId).HasColumnName("accountid");
        builder.Property(x => x.ResourceKey).HasColumnName("resourcekey").HasMaxLength(ColumnMetadata.DefaultNameLength);
        builder.Property(x => x.IdempotencyKey).HasColumnName("idempotencykey").HasMaxLength(ColumnMetadata.DefaultTokenLength).IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.Attempts).HasColumnName("attempts");
        builder.Property(x => x.StartedAt).HasColumnName("startedat");
        builder.Property(x => x.CompletedAt).HasColumnName("completedat");
        builder.Property(x => x.ErrorCode).HasColumnName("errorcode").HasMaxLength(ColumnMetadata.DefaultNameLength);
        builder.Property(x => x.ErrorMessage).HasColumnName("errormessage").HasColumnType(ColumnMetadata.TextField);
        builder.HasIndex(x => new { x.JobKey, x.IdempotencyKey }).IsUnique();
        builder.HasIndex(x => new { x.AccountId, x.StartedAt });
    }
}

public class PublicLinkGrantConfiguration : IEntityTypeConfiguration<PublicLinkGrant>
{
    public void Configure(EntityTypeBuilder<PublicLinkGrant> builder)
    {
        builder.ToTable(name: TableMetadata.PublicLinkGrant, schema: SchemaMetadata.Application);
        builder.Property(x => x.PublicLinkGrantId).HasColumnName("id");
        builder.Property(x => x.AccountId).HasColumnName("accountid");
        builder.Property(x => x.ResourceType).HasColumnName("resourcetype").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.ResourceId).HasColumnName("resourceid").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.Scopes).HasColumnName("scopes").HasMaxLength(ColumnMetadata.DefaultDescriptionLength).IsRequired();
        builder.Property(x => x.Purpose).HasColumnName("purpose").HasMaxLength(ColumnMetadata.DefaultDescriptionLength).IsRequired();
        builder.Property(x => x.SubjectTokenIdHash).HasColumnName("subjecttokenidhash").HasMaxLength(ColumnMetadata.DefaultTokenLength).IsRequired();
        builder.Property(x => x.ExpiresAt).HasColumnName("expiresat");
        builder.Property(x => x.RevokedAt).HasColumnName("revokedat");
        builder.Property(x => x.RevokedBy).HasColumnName("revokedby").HasMaxLength(ColumnMetadata.DefaultNameLength);
        builder.Property(x => x.CreatedByPrincipalId).HasColumnName("createdbyprincipalid").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.AccessCount).HasColumnName("accesscount");
        builder.Property(x => x.LastAccessedAt).HasColumnName("lastaccessedat");
        builder.HasIndex(x => x.SubjectTokenIdHash).IsUnique();
        builder.HasIndex(x => new { x.AccountId, x.ResourceType, x.ResourceId });
    }
}

public class AccountSupportGrantConfiguration : IEntityTypeConfiguration<AccountSupportGrant>
{
    public void Configure(EntityTypeBuilder<AccountSupportGrant> builder)
    {
        builder.ToTable(name: TableMetadata.AccountSupportGrant, schema: SchemaMetadata.Application);
        builder.Property(x => x.AccountSupportGrantId).HasColumnName("id");
        builder.Property(x => x.AccountId).HasColumnName("accountid");
        builder.Property(x => x.SupportUserId).HasColumnName("supportuserid");
        builder.Property(x => x.Reason).HasColumnName("reason").HasMaxLength(ColumnMetadata.DefaultDescriptionLength).IsRequired();
        builder.Property(x => x.TicketReference).HasColumnName("ticketreference").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.ApprovedBy).HasColumnName("approvedby").HasMaxLength(ColumnMetadata.DefaultNameLength);
        builder.Property(x => x.ApprovedAt).HasColumnName("approvedat");
        builder.Property(x => x.AccessLevel).HasColumnName("accesslevel").HasMaxLength(ColumnMetadata.DefaultNameLength).IsRequired();
        builder.Property(x => x.StartsAt).HasColumnName("startsat");
        builder.Property(x => x.EndsAt).HasColumnName("endsat");
        builder.Property(x => x.RevokedAt).HasColumnName("revokedat");
        builder.Property(x => x.RevokedBy).HasColumnName("revokedby").HasMaxLength(ColumnMetadata.DefaultNameLength);
        builder.HasIndex(x => new { x.AccountId, x.SupportUserId, x.StartsAt, x.EndsAt });
    }
}
