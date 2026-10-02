using ActPurchase.Services.Contracts.DTO;

namespace ActPurchase.Services.Contracts.Interfaces
{
    /// <summary>
    /// Сервис для работы с контрагентами
    /// </summary>
    public interface ICounterpartyService
    {
        /// <summary>
        /// олучает список всех контрагентов
        /// </summary>
        Task<IReadOnlyCollection<CounterpartyModel>> GetAllCounterpartiesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получает контрагента по id
        /// </summary>
        Task<CounterpartyModel> GetCounterpartyByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Создаёт нового контрагента
        /// </summary>
        Task<CounterpartyModel> CreateCounterpartyAsync(CounterpartyCreateModel model, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет контрагента по id
        /// </summary>
        Task DeleteCounterpartyAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет данные контрагента
        /// </summary>
        Task UpdateCounterpartyAsync(CounterpartyModel model, CancellationToken cancellationToken);
    }
}
