using Common.Domain.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Configurations;
internal class TransporterGroupConfiguration : IEntityTypeConfiguration<TransporterGroup>
{
    public void Configure(EntityTypeBuilder<TransporterGroup> builder)
    {
        //Table name
        builder.ToTable(name: TableMetadata.TransporterGroup, schema: SchemaMetadata.Application);

        //Column names
        builder.Property(x => x.TransporterId).HasColumnName("transporterid");
        builder.Property(x => x.GroupId).HasColumnName("groupid");

    }
}
