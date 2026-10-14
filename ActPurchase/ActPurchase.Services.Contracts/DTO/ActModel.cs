namespace ActPurchase.Services.Contracts.DTO;

/// <summary>
/// DTO для чтения акта. Это то, что сервер отдаёт клиенту, когда тот запрашивает акт
/// </summary>
public class ActModel : ActCreateModel
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

    /// <summary>
    /// Покупатель
    /// </summary>
    public CounterpartyModel? Buyer { get; set; }

    /// <summary>
    /// Продавец
    /// </summary>
    public CounterpartyModel? Seller { get; set; }

    /// <summary>
    /// Директор
    /// </summary>
    public DirectorModel? Director { get; set; }

    /// <summary>
    /// Позиции акта
    /// </summary>
    public IReadOnlyCollection<ActItemModel> Items { get; set; } = [];
}
