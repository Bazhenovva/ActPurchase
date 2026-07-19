using ActPurchase.DataAccess.Dal.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActPurchase.DataAccess.Context.EntityFrameworkCore;

/// <summary>
/// 
/// </summary>
public static class EntityTypeBuilderExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public static void HasIdAsKey<T>(this EntityTypeBuilder<T> builder)
        where T : class, IEntityWithId
        => builder.HasKey(x => x.Id);

    /// <summary>
    /// 
    /// </summary>
    public static void CreateAuditConfiguration<T>(this EntityTypeBuilder<T> builder)
        where T : class, IEntityAuditCreated
    {
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(200);
    }

    /// <summary>
    /// 
    /// </summary>
    public static void UpdateAuditConfiguration<T>(this EntityTypeBuilder<T> builder)
        where T : class, IEntityAuditUpdate
    {
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.Property(x => x.UpdatedBy).IsRequired().HasMaxLength(200);
    }
}
