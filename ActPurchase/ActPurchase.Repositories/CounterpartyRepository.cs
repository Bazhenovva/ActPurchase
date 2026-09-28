using ActPurchase.Context.Repositories;
using ActPurchase.Dal.Contracts.Repositories;
using ActPurchase.Domain.Entities;
using ActPurchase.Domain.Enum;
using ActPurchase.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ActPurchase.Repositories
{
    /// <summary>
    ///  Репозиторий работы с <see cref="Counterparty"/>
    /// </summary>
    public class CounterpartyRepository : BaseWriteRepository<Counterparty>, ICounterpartyRepository
    {
        private readonly IReader reader;

        /// </summary>
        /// Инициализирует новый экземпляр <see cref="CounterpartyRepository"/>
        /// </summary>
        public CounterpartyRepository(IDbWriterContext writerContext, IReader reader)
        : base(writerContext)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Получает коллекцию контрагентов, отсортированных по названию
        /// </summary>
        Task<IReadOnlyCollection<Counterparty>> ICounterpartyRepository.GetAllCounterpartiesAsync(CancellationToken cancellationToken)
            => reader.Read<Counterparty>()
            .NotDeletedAt()
            .OrderBy(x => x.CompanyName)
            .ToReadOnlyCollectionAsync(cancellationToken);

        /// <summary>
        /// Получает контрагента по идентификатору
        /// </summary>
        Task<Counterparty?> ICounterpartyRepository.GetCounterpartyByIdAsync(Guid id, CancellationToken cancellationToken)
            => reader.Read<Counterparty>()
            .NotDeletedAt()
            .ById(id)
            .FirstOrDefaultAsync(cancellationToken);

        /// <summary>
        /// Получает контрагента по инн
        /// </summary>
        Task<Counterparty?> ICounterpartyRepository.GetCounterpartyByInnAsync(string inn, CancellationToken cancellationToken)
            => reader.Read<Counterparty>()
            .NotDeletedAt()
            .Where(x => x.Inn == inn)
            .FirstOrDefaultAsync(cancellationToken);

        /// <summary>
        /// Получает коллекцию контрагентов по типу
        /// </summary>
        Task<IReadOnlyCollection<Counterparty>> ICounterpartyRepository.GetCounterpartiesByTypeAsync(CounterpartyType type, CancellationToken cancellationToken)
            => reader.Read<Counterparty>()
            .NotDeletedAt()
            .Where(x => x.Type == type)
            .OrderBy(x => x.CompanyName)
            .ToReadOnlyCollectionAsync(cancellationToken);

        /// <summary>
        ///  Получает коллекцию всех покупателей, отсортированных по названию компании
        /// </summary>
        Task<IReadOnlyCollection<Counterparty>> ICounterpartyRepository.GetBuyersAsync(CancellationToken cancellationToken)
            => reader.Read<Counterparty>()
            .NotDeletedAt()
            .Where(x => x.Type == CounterpartyType.Buyer)
            .OrderBy(x => x.CompanyName)
            .ToReadOnlyCollectionAsync(cancellationToken);

        /// <summary>
        /// Получает коллекцию всех продавцов, отсортированных по названию компании
        /// </summary>
        Task<IReadOnlyCollection<Counterparty>> ICounterpartyRepository.GetSellersAsync(CancellationToken cancellationToken)
            => reader.Read<Counterparty>()
                .NotDeletedAt()
                .Where(x => x.Type == CounterpartyType.Seller)
                .OrderBy(x => x.CompanyName)
                .ToReadOnlyCollectionAsync(cancellationToken);


    }
}
