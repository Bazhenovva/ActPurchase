using ActPurchase.Entities;
using ActPurchase.DataAccess.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActPurchase.Entities.Configurations;

public class SellerConfiguration : IEntityTypeConfiguration<Seller>
{
    public void Configure(EntityTypeBuilder<Seller> builder)
    {
        builder.ToTable("Sellers");
        builder.HasIdAsKey();
        builder.CreateAuditConfiguration();
        builder.UpdateAuditConfiguration();

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.PassportSeries)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(x => x.PassportNumber)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(x => x.Address)
            .HasMaxLength(500);
    }
}
