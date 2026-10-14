using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ActPurchase.Context.EntityFrameworkCore;
using ActPurchase.Domain.Entites;

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

            var seedDate = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

            builder.HasData(
                new Act
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Number = "001",
                    City = "Москва",
                    Date = new DateOnly(2026, 1, 15),
                    Type = "Купля-продажа",
                    BuyerId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    SellerId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    DirectorId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    TotalQuantity = 6,
                    TotalSum = 16000,
                    CreatedAt = seedDate,
                    CreatedBy = "System",
                    UpdatedAt = seedDate,
                    UpdatedBy = "System"
                },
                new Act
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Number = "002",
                    City = "Санкт-Петербург",
                    Date = new DateOnly(2026, 2, 20),
                    Type = "Поставка",
                    BuyerId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    SellerId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    DirectorId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    TotalQuantity = 0,
                    TotalSum = 0,
                    CreatedAt = seedDate,
                    CreatedBy = "System",
                    UpdatedAt = seedDate,
                    UpdatedBy = "System"
                }
            );

        }
    }
}
