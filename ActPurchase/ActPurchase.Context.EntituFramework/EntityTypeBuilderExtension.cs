using ActPurchase.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActPurchase.Context.EntityFrameworkCore
{
    /// <summary>
    /// общие методы расширения для <see cref="EntityTypeBuilder{TEntity}"/>
    /// чтобы не писать одинаковые методы в каждой конфигурации сущности
    /// </summary>
    public static class EntityTypeBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static void HasIdAsKey<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class, IEntityWithId
        {
            builder.HasKey(x => x.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CreateAuditConfiguration<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class, IEntityAuditCreated
        {
            builder.Property(x => x.CreatedAt)
                .IsRequired();
            builder.Property(x => x.CreatedBy)
                .IsRequired()
                .HasMaxLength(150);
        }
        /// <summary>
        /// 
        /// </summary>
        public static void UpdateAuditConfiguration<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class, IEntityAuditUpdated
        {
            builder.Property(x => x.UpdatedAt)
                .IsRequired();
            builder.Property(x => x.UpdatedBy)
                .IsRequired()
                .HasMaxLength(150);
        }
    }
}
