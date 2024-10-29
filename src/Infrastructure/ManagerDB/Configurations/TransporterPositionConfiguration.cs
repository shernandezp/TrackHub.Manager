using Common.Domain.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Configurations;
public sealed class TransporterPositionConfiguration : IEntityTypeConfiguration<TransporterPosition>
{
    public void Configure(EntityTypeBuilder<TransporterPosition> builder)
    {
        //Table name
        builder.ToTable(name: TableMetadata.TransporterPosition, schema: SchemaMetadata.Application);

        //Column names
        builder.Property(x => x.TransporterPositionId).HasColumnName("id");
        builder.Property(x => x.TransporterId).HasColumnName("transporterid");
        builder.Property(x => x.GeometryId).HasColumnName("geometryid");
        builder.Property(x => x.Latitude).HasColumnName("latitude");
        builder.Property(x => x.Longitude).HasColumnName("longitude");
        builder.Property(x => x.Altitude).HasColumnName("altitude");
        builder.Property(x => x.DateTime).HasColumnName("datetime");
        builder.Property(x => x.Offset).HasColumnName("offset").HasColumnType("interval");
        builder.Property(x => x.Speed).HasColumnName("speed");
        builder.Property(x => x.Course).HasColumnName("course");
        builder.Property(x => x.EventId).HasColumnName("eventId");
        builder.Property(x => x.Address).HasColumnName("address");
        builder.Property(x => x.City).HasColumnName("city");
        builder.Property(x => x.State).HasColumnName("state");
        builder.Property(x => x.Country).HasColumnName("country");

        builder.Property(e => e.Attributes)
            .HasColumnName("attributes")
            .HasColumnType("json");

    }
}
