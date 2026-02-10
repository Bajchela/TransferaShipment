using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shipments.Domain.Models;

namespace Shipments.Infrastructure.Persistence.Configurations;

public class ShipmentConfig : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> b)
    {
        b.ToTable("Shipments");

        b.HasKey(x => x.Id);

        b.Property(x => x.ReferenceNumber)
            .IsRequired()
            .HasMaxLength(50);

        b.HasIndex(x => x.ReferenceNumber).IsUnique();

        b.Property(x => x.Sender).IsRequired().HasMaxLength(200);
        b.Property(x => x.Recipient).IsRequired().HasMaxLength(200);

        b.Property(x => x.Status).IsRequired();

        b.Property(x => x.CreatedAt).IsRequired();
    }
}
