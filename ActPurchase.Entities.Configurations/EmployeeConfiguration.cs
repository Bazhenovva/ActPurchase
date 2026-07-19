using ActPurchase.Entities;
using ActPurchase.DataAccess.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActPurchase.Entities.Configurations;

/// <summary>
/// Конфигурация сущности <see cref="Employee"/> для Entity Framework Core
/// </summary>
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    /// <summary>
    /// 
    /// </summary>
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasIdAsKey();
        builder.CreateAuditConfiguration();
        builder.UpdateAuditConfiguration();

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Position)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.PowerOfAttorneyNumber)
            .HasMaxLength(100);
    }
}
