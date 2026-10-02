using ActPurchase.Services.Contracts.DTO;

namespace ActPurchase.Services.Contracts.Interfaces
{
    /// <summary>
    /// Сервис для работы с актами
    /// </summary>
    public interface IActService
    {
        /// <summary>
        /// Получает все акты
        /// </summary>
        Task<IReadOnlyCollection<ActModel>> GetAllActsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получает по айди
        /// </summary>
        Task<ActModel> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Создает новый акт
        /// </summary>
        Task<ActModel> CreateActAsync(ActCreateModel model, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет акт по айди
        /// </summary>
        Task DeleteActAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет данные акта
        /// </summary>
        Task UpdateActAsync(ActModel model, CancellationToken cancellationToken);
    }
}
