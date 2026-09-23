using ActPurchase.Dal.Contracts.Repositories;
using ActPurchase.Domain.Entities;

namespace ActPurchase.Repositories.Contracts
{
    /// <summary>
    /// Репозиторий работы с <see cref="ActItem"/>
    /// </summary>
    public interface IActItemRepository
    {
        /// <summary>
        /// получение колекции всех позиций
        /// </summary>
        Task<IReadOnlyCollection<ActItem>> GetAllActItemsAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получает позицию по идентификатору
        /// </summary>
        Task<ActItem?> GetActItemByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
