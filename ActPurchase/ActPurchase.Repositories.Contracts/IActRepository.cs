using ActPurchase.Dal.Contracts.Repositories;
using ActPurchase.Domain.Entities;

namespace ActPurchase.Repositories.Contracts
{
    /// <summary>
    ///  Репозиторий работы с <see cref="Act"/>
    /// </summary>
    public interface IActRepository : IBaseWriteRepository<Act>
    {
        /// <summary>
        /// получение колекции всех актов
        /// </summary>
        Task<IReadOnlyCollection<Act>> GetAllActsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// получение акта по айди
        /// </summary>
        Task<Act?> GetActByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// получение акта по номеру
        /// </summary>
        Task<Act?> GetActByNumberAsync(string number, CancellationToken cancellationToken);

    }
}
