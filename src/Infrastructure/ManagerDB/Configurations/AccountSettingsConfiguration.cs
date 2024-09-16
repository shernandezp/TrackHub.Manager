using Common.Domain.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Configurations;

public class AccountSettingsConfiguration : IEntityTypeConfiguration<AccountSettings>
{
    public void Configure(EntityTypeBuilder<AccountSettings> builder)
    {
        //Table name
        builder.ToTable(name: TableMetadata.AccountSettings, schema: SchemaMetadata.Application);
        builder.HasKey(x => x.AccountId);

        //Column names
        builder.Property(x => x.AccountId).HasColumnName("id");
        builder.Property(x => x.Maps).HasColumnName("maps");
        builder.Property(x => x.StoreLastPosition).HasColumnName("storelastposition");

    }
}
