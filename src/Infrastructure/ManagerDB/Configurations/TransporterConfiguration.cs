﻿using Common.Domain.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Configurations;
public sealed class TransporterConfiguration : IEntityTypeConfiguration<Transporter>
{
    public void Configure(EntityTypeBuilder<Transporter> builder)
    {
        //Table name
        builder.ToTable(name: TableMetadata.Transporter, schema: SchemaMetadata.Application);

        //Column names
        builder.Property(x => x.TransporterId).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.TransporterTypeId).HasColumnName("transportertypeid");

        builder.Property(t => t.Name)
            .HasMaxLength(ColumnMetadata.DefaultNameLength)
            .IsRequired();

        builder
            .HasMany(d => d.Devices)
            .WithOne(d => d.Transporter)
            .HasForeignKey(d => d.TransporterId)
            .IsRequired(false);

        builder
            .HasOne(d => d.Position)
            .WithOne(d => d.Transporter)
            .HasForeignKey<TransporterPosition>(d => d.TransporterId)
            .IsRequired(false);
    }
}
