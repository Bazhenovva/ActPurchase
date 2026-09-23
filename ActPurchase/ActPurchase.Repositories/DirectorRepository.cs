using ActPurchase.Repositories.Contracts;
using ActPurchase.Context.Repositories;
using ActPurchase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ActPurchase.Dal.Contracts.Repositories;

namespace ActPurchase.Repositories
{
    /// <summary>
    /// Репозиторий работы с <see cref="Director"/>
    /// </summary>
    public class DirectorRepository : BaseWriteRepository<Director>, IDirectorRepository
    {
        private readonly IReader reader;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="DirectorRepository"/>
        /// </summary>
        public DirectorRepository(IDbWriterContext writerContext, IReader reader)
            : base(writerContext)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Получает коллекцию директоров, отсортированных по фамилии
        /// </summary>
        Task<IReadOnlyCollection<Director>> IDirectorRepository.GetAllDirectorsAsync(CancellationToken cancellationToken)
            => reader.Read<Director>()
            .NotDeletedAt()
            .OrderBy(x => x.LastName)
            .ToReadOnlyCollectionAsync(cancellationToken);

        /// <summary>
        /// Получает директора по идентификатору
        /// </summary>
        Task<Director?> IDirectorRepository.GetDirectorByIdAsync(Guid id, CancellationToken cancellationToken)
            => reader.Read<Director>()
            .NotDeletedAt()
            .ById(id)
            .FirstOrDefaultAsync(cancellationToken);

        /// <summary>
        /// Получает коллекцию директоров указанной компании, отсортированных по фамилии
        /// </summary>
        Task<IReadOnlyCollection<Director>> IDirectorRepository.GetDirectorsByCompanyNameAsync(string companyName, CancellationToken cancellationToken)
            => reader.Read<Director>()
            .NotDeletedAt()
            .Where(x => x.CompanyName == companyName)
            .OrderBy(x => x.LastName)
            .ToReadOnlyCollectionAsync(cancellationToken);

        /// <summary>
        /// Получает директора по фамилии и имени
        /// </summary>
        Task<Director?> IDirectorRepository.GetDirectorByFullNameAsync(string lastName, string firstName, CancellationToken cancellationToken)
            => reader.Read<Director>()
            .NotDeletedAt()
            .Where(x => x.LastName == lastName && x.FirstName == firstName)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
