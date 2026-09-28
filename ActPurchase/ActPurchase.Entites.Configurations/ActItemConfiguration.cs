using ActPurchase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ActPurchase.Context.EntityFrameworkCore;

namespace ActPurchase.Entites.Configurations
{

    /// <summary>
    /// Конфигурация сущности <see cref="ActItem"/> для Entity Framework Core
    /// </summary>
    public class ActItemConfiguration : IEntityTypeConfiguration<ActItem>
    {
        /// <summary>
        /// Описание правил, свойств и связей
        /// </summary>
        public void Configure(EntityTypeBuilder<ActItem> builder)
        {
            builder.ToTable("ActItems");

            builder.HasIdAsKey();
            builder.CreateAuditConfiguration();
            builder.UpdateAuditConfiguration();

            builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(75);

            builder.Property(x => x.Quantity)
            .IsRequired();

            builder.Property(x => x.Price)
            .IsRequired()
            .HasPrecision(8, 2);

            builder.Property(x => x.Sum)
            .IsRequired()
            .HasPrecision(8, 2);

            builder.HasOne<Act>()
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.ActId);
        }
    }
}
