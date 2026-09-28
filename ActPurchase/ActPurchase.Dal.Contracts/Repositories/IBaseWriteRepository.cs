using ActPurchase.Domain.Interfaces;

namespace ActPurchase.Dal.Contracts.Repositories
{
    /// <summary>
    /// Интерфейс записи данных в контекст
    /// </summary>
    public interface IBaseWriteRepository<T> where T : class, IEntityWithId
    {
        /// <summary>
        /// Создаёт новую сущность в контексте
        /// </summary>
        void Create(T entity);

        /// <summary>
        /// Обновляет сущность в контексте
        /// </summary>
        void Update(T entity);

        /// <summary>
        /// Удаляет сущность из контекста
        /// </summary>
        void Delete(T entity);

    }
}
