using ActPurchase.Dal.Contracts.Repositories;
using ActPurchase.Domain.Entities;
using ActPurchase.Domain.Enum;

namespace ActPurchase.Repositories.Contracts
{
    /// <summary>
    ///  Репозиторий работы с <see cref="Counterparty"/>
    /// </summary>
    public interface ICounterpartyRepository : IBaseWriteRepository<Counterparty>
    {

        /// <summary>
        /// получение колекции всех контрагентов
        /// </summary>
        Task<IReadOnlyCollection<Counterparty>> GetAllCounterpartiesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// получение контрагентов по айди
        /// </summary>
        Task<Counterparty?> GetCounterpartyByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// получение контрагентов по инн
        /// </summary>
        Task<Counterparty?> GetCounterpartyByInnAsync(string inn, CancellationToken cancellationToken);

        /// <summary>S
        /// получение контрагента по типу
        /// </summary>
        Task<IReadOnlyCollection<Counterparty>> GetCounterpartiesByTypeAsync(CounterpartyType type, CancellationToken cancellationToken);

        /// <summary>
        /// получает всех покупателей
        /// </summary>
        Task<IReadOnlyCollection<Counterparty>> GetBuyersAsync(CancellationToken cancellationToken);

        /// <summary>
        /// получает всех продавцов
        /// </summary>
        Task<IReadOnlyCollection<Counterparty>> GetSellersAsync(CancellationToken cancellationToken);

    }
}
