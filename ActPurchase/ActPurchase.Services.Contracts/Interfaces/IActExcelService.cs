using ActPurchase.Services.Contracts.DTO;

namespace ActPurchase.Services.Contracts.Interfaces
{
    /// <summary>
    /// Сервис генерации Excel-файлов актов
    /// </summary>
    public interface IActExcelService
    {
        /// <summary>
        /// Генерация Excel-файла по акту
        /// </summary>
        Task<Stream> GenerateActExcelAsync(ActModel act, CancellationToken cancellationToken);
    }
}
