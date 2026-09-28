using ActPurchase.Domain.Interfaces;

namespace ActPurchase.Dal.Contracts.Repositories
{
    /// <summary>
    /// контракт для чтения данных из хранилища, возвращает IQueryable для построения запросов
    /// </summary>
    public interface IReader
    {
        /// <summary>
        /// возвращает запрос на чтение сущностей типа TEntity
        /// </summary>
        IQueryable<TEntity> Read<TEntity>() where TEntity : class, IEntityWithId;
    }
}
