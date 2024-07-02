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
        builder.Property(x => x.DeviceId).HasColumnName("deviceid");
        builder.Property(x => x.OperatorId).HasColumnName("operatorid");

    }
}
