using ActPurchase.Dal.Contracts.Repositories;
using ActPurchase.Domain.Entites;

namespace ActPurchase.Repositories.Contracts
{
    /// <summary>
    /// Репозиторий работы с <see cref="ActItem"/>
    /// </summary>
    public interface IActItemRepository : IBaseWriteRepository<ActItem>
    {
        /// <summary>
        /// получение коллекции всех позиций
        /// </summary>
        Task<IReadOnlyCollection<ActItem>> GetAllActItemsAsync(Guid actId, CancellationToken cancellationToken);

        /// <summary>
        /// Получает позицию по идентификатору
        /// </summary>
        Task<ActItem?> GetActItemByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
