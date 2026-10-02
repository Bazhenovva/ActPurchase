namespace ActPurchase.Services.Contracts.DTO;

/// <summary>
/// DTO для создания. Это то, что клиент присылает серверу
/// </summary>
public class ActCreateModel
{
    /// <summary>
    /// Номер акта
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Город составления
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Дата составления
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Тип акта
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор покупателя
    /// </summary>
    public Guid BuyerId { get; set; }

    /// <summary>
    /// Идентификатор продавца
    /// </summary>
    public Guid SellerId { get; set; }

    /// <summary>
    /// Идентификатор директора
    /// </summary>
    public Guid DirectorId { get; set; }
}
