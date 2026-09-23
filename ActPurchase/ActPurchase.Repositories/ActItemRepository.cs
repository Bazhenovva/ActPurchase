using ActPurchase.Repositories.Contracts;
using ActPurchase.Context.Repositories;
using ActPurchase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ActPurchase.Dal.Contracts.Repositories;

namespace ActPurchase.Repositories
{
    /// <summary>
    ///  Репозиторий работы с <see cref="ActItem"/>
    /// </summary>
    public class ActItemRepository : BaseWriteRepository<ActItem>, IActItemRepository
    {
        private readonly IReader reader;

        /// </summary>
        /// Инициализирует новый экземпляр <see cref="ActRepository"/>
        /// </summary>
        public ActItemRepository(IDbWriterContext writerContext, IReader reader)
        : base(writerContext)
        {
            this.reader = reader;
        }

        /// <summary>
        ///  Получает коллекцию позиций для указанного акта
        /// </summary>
        Task<IReadOnlyCollection<ActItem>> IActItemRepository.GetAllActItemsAsync(Guid actId, CancellationToken cancellationToken)
            => reader.Read<ActItem>()
            .NotDeletedAt()
            .Where(x => x.ActId == actId)
            .ToReadOnlyCollectionAsync(cancellationToken);

        /// <summary>
        /// Получает позицию по идентификатору
        /// </summary>
        Task<ActItem?> IActItemRepository.GetActItemByIdAsync(Guid id, CancellationToken cancellationToken)
            => reader.Read<ActItem>()
            .NotDeletedAt()
            .ById(id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
