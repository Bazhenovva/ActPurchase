using ActPurchase.Context.EntityFrameworkCore;
using ActPurchase.Domain.Entities;
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
        }
    }
}
