using ActPurchase.Entities;
using ActPurchase.DataAccess.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActPurchase.Entities.Configurations;

/// <summary>
/// Конфигурация сущности <see cref="Act"/> для Entity Framework Core
/// </summary>
public class ActConfiguration : IEntityTypeConfiguration<Act>
{
    public void Configure(EntityTypeBuilder<Act> builder)
    {
        builder.ToTable("Acts");
        builder.HasIdAsKey();
        builder.CreateAuditConfiguration();
        builder.UpdateAuditConfiguration();

        builder.Property(x => x.Number)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasIndex(x => x.Number)
            .HasDatabaseName("IX_ActsNumber")
            .IsUnique();
    }
}
