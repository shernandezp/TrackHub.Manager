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
        builder.Property(x => x.MapsKey).HasColumnName("mapskey");
        builder.Property(x => x.OnlineTimeLapse).HasColumnName("onlinetimelapse");
        builder.Property(x => x.StoreLastPosition).HasColumnName("storelastposition");
        builder.Property(x => x.StoringTimeLapse).HasColumnName("storingtimelapse");
        builder.Property(x => x.RefreshMap).HasColumnName("refreshmap");
        builder.Property(x => x.RefreshMapTimer).HasColumnName("refreshmaptimer");

    }
}
