using Common.Domain.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TrackHub.Manager.Infrastructure.Configurations;
internal class DeviceOperatorConfiguration : IEntityTypeConfiguration<DeviceOperator>
{
    public void Configure(EntityTypeBuilder<DeviceOperator> builder)
    {
        //Table name
        builder.ToTable(name: TableMetadata.DeviceOperator, schema: SchemaMetadata.Application);

        //Column names
        builder.Property(x => x.DeviceOperatorId).HasColumnName("id");
        builder.Property(x => x.Identifier).HasColumnName("identifier");
        builder.Property(x => x.Serial).HasColumnName("serial");
        builder.Property(x => x.DeviceId).HasColumnName("deviceid");
        builder.Property(x => x.OperatorId).HasColumnName("operatorid");

        builder.Property(t => t.Serial)
            .HasMaxLength(ColumnMetadata.DefaultFieldLength)
            .IsRequired();

        builder.Property(t => t.Identifier)
            .HasMaxLength(ColumnMetadata.DefaultFieldLength)
            .IsRequired();

        builder
            .HasOne(e => e.Operator)
            .WithMany(e => e.DeviceOperator)
            .HasForeignKey(e => e.OperatorId);

    }
}
