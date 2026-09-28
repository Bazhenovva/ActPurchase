using System.Collections.ObjectModel;
using ActPurchase.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ActPurchase.Context.Repositories
{
    /// <summary>
    /// статический класс с методами расширения для IQueryable<T>
    /// чтобы в репозиториях не писать одно и то же
    /// </summary>
    public static class CommonSpecs
    {
        /// <summary>
        /// Превращает запрос в готовую коллекцию
        /// </summary>
        public static async Task<IReadOnlyCollection<TEntity>> ToReadOnlyCollectionAsync<TEntity>
            (this IQueryable<TEntity> query, CancellationToken cancellationToken)
        {
            var list = await query.ToListAsync(cancellationToken);
            return new ReadOnlyCollection<TEntity>(list);
        }

        /// <summary>
        /// Добавляет к запросу фильтр: только неудалённые
        /// </summary>
        public static IQueryable<TEntity> NotDeletedAt<TEntity>(this IQueryable<TEntity> query)
            where TEntity : class, IEntityAuditDeletedAt => query.Where(x => x.DeletedAt == null);

        /// <summary>
        /// Добавляет к запросу фильтр по Id
        /// </summary>
        public static IQueryable<TEntity> ById<TEntity>(this IQueryable<TEntity> query, Guid id)
            where TEntity : class, IEntityWithId => query.Where(x => x.Id == id);
    }
}
