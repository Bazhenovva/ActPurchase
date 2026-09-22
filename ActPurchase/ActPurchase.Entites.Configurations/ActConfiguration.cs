using ActPurchase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ActPurchase.Context.EntityFrameworkCore;

namespace ActPurchase.Entites.Configurations
{

    /// <summary>
    /// Конфигурация сущности <see cref="Act"/> для Entity Framework Core
    /// </summary>
    public class ActConfiguration : IEntityTypeConfiguration<Act>
    {
        /// <summary>
        /// Описание правил, свойств и связей
        /// </summary>
        public void Configure(EntityTypeBuilder<Act> builder)
        {
            builder.ToTable("Acts");

            builder.HasIdAsKey();
            builder.CreateAuditConfiguration();
            builder.UpdateAuditConfiguration();

            builder.Property(x => x.Number)
            .IsRequired()
            .HasMaxLength(15);

            builder.Property(x => x.City)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Date)
                .IsRequired();

            builder.Property(x => x.TotalQuantity)
                .IsRequired();

            builder.Property(x => x.TotalSum)
                .HasPrecision(8, 2);


            builder.HasOne(x => x.Buyer)
                .WithMany()
                .HasForeignKey(x => x.BuyerId);

            builder.HasOne(x => x.Seller)
                .WithMany()
                .HasForeignKey(x => x.SellerId);

            builder.HasOne(x => x.Director)
                .WithMany()
                .HasForeignKey(x => x.DirectorId);

            builder.HasMany(x => x.Items)
                .WithOne()
                .HasForeignKey(x => x.ActId);

            builder.HasIndex(x => x.Number)
                .HasDatabaseName("IX_ActsNumber")
                .IsUnique();

        }
    }
}
