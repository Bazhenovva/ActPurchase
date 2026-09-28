namespace ActPurchase.Dal.Contracts.Repositories
{
    /// <summary>
    /// Интерфейс единицы работы для сохранения изменений
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// cохраняет все изменения в бд, где cancellationToken - маркер отмены действия
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
