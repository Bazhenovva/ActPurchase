using ActPurchase.Services.Contracts.DTO;

namespace ActPurchase.Services.Contracts.Interfaces
{
    /// <summary>
    /// Сервис для работы с директорами
    /// </summary>
    public interface IDirectorService
    {
        /// <summary>
        /// Получает список всех директоров
        /// </summary>
        Task<IReadOnlyCollection<DirectorModel>> GetAllDirectorsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получает директора по айди
        /// </summary>
        Task<DirectorModel> GetDirectorByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Создаёт нового директора
        /// </summary>
        Task<DirectorModel> CreateDirectorAsync(DirectorCreateModel model, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет директора по айди
        /// </summary>
        Task DeleteDirectorAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет данные директора
        /// </summary>
        Task UpdateDirectorAsync(DirectorModel model, CancellationToken cancellationToken);
    }
}
