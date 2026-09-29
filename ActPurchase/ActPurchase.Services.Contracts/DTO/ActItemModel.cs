namespace ActPurchase.Services.Contracts.DTO;

/// <summary>
/// DTO для чтения акта. Это то, что сервер отдаёт клиенту, когда тот запрашивает акт
/// </summary>
public class ActItemModel : ActItemCreateModel
{
    /// <summary>
    /// Идентификатор позиции
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Сумма
    /// </summary>
    public decimal Sum { get; set; }
}
