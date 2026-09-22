using ActPurchase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ActPurchase.Context.EntityFrameworkCore;

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
        }
    }
}
