using ActPurchase.Repositories.Contracts;
using ActPurchase.Context.Repositories;
using ActPurchase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ActPurchase.Dal.Contracts.Repositories;

namespace ActPurchase.Repositories
{
    /// <summary>
    ///  Репозиторий работы с <see cref="Act"/>
    /// </summary>
    public class ActRepository : BaseWriteRepository<Act>, IActRepository
    {
        private readonly IReader reader;

        /// </summary>
        /// Инициализирует новый экземпляр <see cref="ActRepository"/>
        /// </summary>
        public ActRepository(IDbWriterContext writerContext, IReader reader)
        : base(writerContext)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Получает коллекцию актов, отсортированных по номеру
        /// </summary>
        Task<IReadOnlyCollection<Act>> IActRepository.GetAllActsAsync(CancellationToken cancellationToken)
            => reader.Read<Act>()
            .NotDeletedAt()
            .OrderBy(x => x.Number)
            .ToReadOnlyCollectionAsync(cancellationToken);

        /// <summary>
        /// Получает акт по идентификатору
        /// </summary>
        Task<Act?> IActRepository.GetActByIdAsync(Guid id, CancellationToken cancellationToken)
            => reader.Read<Act>()
            .NotDeletedAt()
            .ById(id)
            .FirstOrDefaultAsync(cancellationToken);

        /// <summary>
        /// Получает акт по номеру
        /// </summary>
        Task<Act?> IActRepository.GetActByNumberAsync(string number, CancellationToken cancellationToken)
            => reader.Read<Act>()
            .NotDeletedAt()
            .Where(x => x.Number == number)
            .FirstOrDefaultAsync(cancellationToken);

    }
}
