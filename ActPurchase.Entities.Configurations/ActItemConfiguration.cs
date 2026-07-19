using ActPurchase.Entities;
using ActPurchase.DataAccess.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActPurchase.Entities.Configurations;

public class ActItemConfiguration : IEntityTypeConfiguration<ActItem>
{
    public void Configure(EntityTypeBuilder<ActItem> builder)
    {
        builder.ToTable("ActItems");
        builder.HasIdAsKey();
        builder.CreateAuditConfiguration();
        builder.UpdateAuditConfiguration();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Unit)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Sum)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
    }
}
