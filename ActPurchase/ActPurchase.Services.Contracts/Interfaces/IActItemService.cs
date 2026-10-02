using ActPurchase.Services.Contracts.DTO;

namespace ActPurchase.Services.Contracts.Interfaces
{
    /// <summary>
    /// Сервис для работы с позициями актов
    /// </summary>
    public interface IActItemService
    {
        /// <summary>
        /// Получает список всех позиций
        /// </summary>
        Task<IReadOnlyCollection<ActItemModel>> GetAllActItemsAsync(Guid actId, CancellationToken cancellationToken);

        /// <summary>
        /// Получает позицию по идентификатору
        /// </summary>
        Task<ActItemModel> GetActItemByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Создаёт новую позицию
        /// </summary>
        Task<ActItemModel> CreateActItemAsync(ActItemCreateModel model, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет позицию по id
        /// </summary>
        Task DeleteActItemAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет данные позиции
        /// </summary>
        Task UpdateActItemAsync (ActItemModel model, CancellationToken cancellationToken);
    }
}
