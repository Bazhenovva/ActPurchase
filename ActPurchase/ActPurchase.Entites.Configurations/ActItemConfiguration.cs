using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ActPurchase.Context.EntityFrameworkCore;
using ActPurchase.Domain.Entites;

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

            var seedDate = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

            builder.HasData(
                new ActItem
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    ActId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Стол письменный",
                    Quantity = 2,
                    Price = 5000,
                    Sum = 10000,
                    CreatedAt = seedDate,
                    CreatedBy = "System",
                    UpdatedAt = seedDate,
                    UpdatedBy = "System"
                },
                new ActItem
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    ActId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Стул офисный",
                    Quantity = 4,
                    Price = 1500,
                    Sum = 6000,
                    CreatedAt = seedDate,
                    CreatedBy = "System",
                    UpdatedAt = seedDate,
                    UpdatedBy = "System"
                }
            );
        }
    }
}
