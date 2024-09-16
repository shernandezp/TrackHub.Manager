using Common.Domain.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Configurations;
public sealed class UserSettingsConfiguration : IEntityTypeConfiguration<UserSettings>
{
    public void Configure(EntityTypeBuilder<UserSettings> builder)
    {
        //Table name
        builder.ToTable(name: TableMetadata.UserSettings, schema: SchemaMetadata.Application);

        builder.HasKey(x => x.UserId);

        //Column names
        builder.Property(x => x.UserId).HasColumnName("id");
        builder.Property(x => x.Language).HasColumnName("language");
        builder.Property(x => x.Style).HasColumnName("style");

    }
}
