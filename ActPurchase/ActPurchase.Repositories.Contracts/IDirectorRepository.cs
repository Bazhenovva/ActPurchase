using ActPurchase.Dal.Contracts.Repositories;
using ActPurchase.Domain.Entities;

namespace ActPurchase.Repositories.Contracts
{
    /// <summary>
    ///  Репозиторий работы с <see cref="Director"/>
    /// </summary>
    public interface IDirectorRepository : IBaseWriteRepository<Director>
    {
        /// <summary>
        /// Получает коллекцию всех директоров
        /// </summary>
        Task<IReadOnlyCollection<Director>> GetAllDirectorsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получает директора по идентификатору
        /// </summary>
        Task<Director?> GetDirectorByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получает коллекцию директоров указанной компании, отсортированных по фамилии
        /// </summary>
        Task<IReadOnlyCollection<Director>> GetDirectorsByCompanyNameAsync(string companyName, CancellationToken cancellationToken);

        /// <summary>
        /// Получает директора по фамилии и имени
        /// </summary>
        Task<Director?> GetDirectorByFullNameAsync(string lastName, string firstName, CancellationToken cancellationToken);
    }
}
