namespace ActPurchase.Api.Models;

/// <summary>
/// DTO для чтения акта. Это то, что сервер отдаёт клиенту, когда тот запрашивает акт
/// </summary>
public class ActApiModel : ActCreateApiModel
{
    /// <summary>
    /// Идентификатор акта
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Итоговое количество
    /// </summary>
    public int TotalQuantity { get; set; }

    /// <summary>
    /// Итоговая сумма
    /// </summary>
    public decimal TotalSum { get; set; }
}
