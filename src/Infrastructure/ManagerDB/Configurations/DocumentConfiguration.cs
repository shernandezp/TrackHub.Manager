using Common.Domain.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackHub.Manager.Infrastructure.Entities;

namespace TrackHub.Manager.Infrastructure.Configurations;

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
