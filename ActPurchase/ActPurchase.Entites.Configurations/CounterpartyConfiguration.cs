using ActPurchase.Context.EntityFrameworkCore;
using ActPurchase.Domain.Entites;
using ActPurchase.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActPurchase.Entites.Configurations
{
    /// <summary>
    /// Конфигурация сущности <see cref="Counterparty"/> для Entity Framework Core
    /// </summary>
    public class CounterpartyConfiguration : IEntityTypeConfiguration<Counterparty>
    {
        /// <summary>
        /// Описание правил, свойств и связей
        /// </summary>
        public void Configure(EntityTypeBuilder<Counterparty> builder)
        {
            builder.ToTable("Counterparties");

            builder.HasIdAsKey();
            builder.CreateAuditConfiguration();
            builder.UpdateAuditConfiguration();

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(x => x.CompanyName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Inn)
                .IsRequired()
                .HasMaxLength(12);

            builder.Property(x => x.Kpp)
                .IsRequired()
                .HasMaxLength(9);

            builder.Property(x => x.Ogrn)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(x => x.CheckingAccount)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.CorrespondentAccount)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Bik)
                .IsRequired()
                .HasMaxLength(9);

            builder.Property(x => x.BankName)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.LegalAddress)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.Inn)
                .HasDatabaseName("IX_CounterpartiesInn")
                .IsUnique();

            var seedDate = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

            builder.HasData(
                new Counterparty
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Type = CounterpartyType.Buyer,
                    CompanyName = "ООО Озон",
                    Inn = "1234567890",
                    Kpp = "123456789",
                    Ogrn = "1234567890123",
                    CheckingAccount = "40817810099910004312",
                    CorrespondentAccount = "30101810400000000225",
                    Bik = "044525225",
                    BankName = "Сбербанк",
                    LegalAddress = "г. Москва, ул. Ленина, д. 1",
                    CreatedAt = seedDate,
                    CreatedBy = "System",
                    UpdatedAt = seedDate,
                    UpdatedBy = "System"
                },
                new Counterparty
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Type = CounterpartyType.Seller,
                    CompanyName = "ООО Вб",
                    Inn = "0987654321",
                    Kpp = "987654321",
                    Ogrn = "3210987654321",
                    CheckingAccount = "40817810799910005432",
                    CorrespondentAccount = "30101810400000000225",
                    Bik = "044525225",
                    BankName = "ВТБ",
                    LegalAddress = "г. Санкт-Петербург, ул. Пушкина, д. 10",
                    CreatedAt = seedDate,
                    CreatedBy = "System",
                    UpdatedAt = seedDate,
                    UpdatedBy = "System"
                }
            );
        }
    }
}
