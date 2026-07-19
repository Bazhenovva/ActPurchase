using ActPurchase.Entities;
using ActPurchase.DataAccess.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActPurchase.Entities.Configurations;

public class BuyerConfiguration : IEntityTypeConfiguration<Buyer>
{
    public void Configure(EntityTypeBuilder<Buyer> builder)
    {
        builder.ToTable("Buyers");
        builder.HasIdAsKey();
        builder.CreateAuditConfiguration();
        builder.UpdateAuditConfiguration();

        builder.Property(x => x.CompanyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Inn)
            .IsRequired()
            .HasMaxLength(12);

        builder.Property(x => x.Kpp)
            .HasMaxLength(9);

        builder.Property(x => x.LegalAddress)
            .HasMaxLength(500);
    }
}
