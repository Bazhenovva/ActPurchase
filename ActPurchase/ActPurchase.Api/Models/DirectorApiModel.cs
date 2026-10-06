namespace ActPurchase.Api.Models;

/// <summary>
/// DTO для чтения директора. Это то, что сервер отдаёт клиенту
/// </summary>
public class DirectorApiModel : DirectorCreateApiModel
{
    /// <summary>
    /// Идентификатор директора
    /// </summary>
    public Guid Id { get; set; }
}
