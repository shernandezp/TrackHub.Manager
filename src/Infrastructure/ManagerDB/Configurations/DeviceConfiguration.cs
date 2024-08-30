using Common.Domain.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Configurations;
internal class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        //Table name
        builder.ToTable(name: TableMetadata.Device, schema: SchemaMetadata.Application);

        //Column names
        builder.Property(x => x.DeviceId).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Identifier).HasColumnName("identifier");
        builder.Property(x => x.Serial).HasColumnName("serial");
        builder.Property(x => x.DeviceTypeId).HasColumnName("devicetypeid");
        builder.Property(x => x.Description).HasColumnName("description");
        builder.Property(x => x.TransporterId).HasColumnName("transporterid");
        builder.Property(x => x.OperatorId).HasColumnName("operatorid");

        builder.Property(t => t.Serial)
            .HasMaxLength(ColumnMetadata.DefaultFieldLength)
            .IsRequired();

        builder.Property(t => t.Identifier)
            .HasMaxLength(ColumnMetadata.DefaultFieldLength)
            .IsRequired();

        builder
            .HasOne(e => e.Operator)
            .WithMany(e => e.Devices)
            .HasForeignKey(e => e.OperatorId);

    }
}
