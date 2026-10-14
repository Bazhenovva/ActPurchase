using ActPurchase.Context.EntityFrameworkCore;
using ActPurchase.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActPurchase.Entites.Configurations
{

    /// <summary>
    /// Конфигурация сущности <see cref="Counterparty"/> для Entity Framework Core
    /// </summary>
    public class DirectorConfiguration : IEntityTypeConfiguration<Director>
    {
        /// <summary>
        /// Описание правил, свойств и связей
        /// </summary>
        public void Configure(EntityTypeBuilder<Director> builder)
        {
            builder.ToTable("Directors");

            builder.HasIdAsKey();
            builder.CreateAuditConfiguration();
            builder.UpdateAuditConfiguration();

            builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(15);

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(x => x.MiddleName)
                .HasMaxLength(20);

            builder.Property(x => x.CompanyName)
                .IsRequired()
                .HasMaxLength(100);

            var seedDate = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

            builder.HasData(
                new Director
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    LastName = "Иванов",
                    FirstName = "Иван",
                    MiddleName = "Иванович",
                    CompanyName = "ООО Озон",
                    CreatedAt = seedDate,
                    CreatedBy = "System",
                    UpdatedAt = seedDate,
                    UpdatedBy = "System"
                },
                new Director
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    LastName = "Петров",
                    FirstName = "Пётр",
                    MiddleName = "Петрович",
                    CompanyName = "ООО Вб",
                    CreatedAt = seedDate,
                    CreatedBy = "System",
                    UpdatedAt = seedDate,
                    UpdatedBy = "System"
                }
            );
        }
    }
}
