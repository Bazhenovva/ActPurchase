namespace ActPurchase.Services.Contracts.DTO;

/// <summary>
/// DTO для чтения директора. Это то, что сервер отдаёт клиенту
/// </summary>
public class DirectorModel : DirectorCreateModel
{
    /// <summary>
    /// Идентификатор директора
    /// </summary>
    public Guid Id { get; set; }
}
