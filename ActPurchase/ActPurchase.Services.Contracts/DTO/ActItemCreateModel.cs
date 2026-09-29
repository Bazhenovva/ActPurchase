namespace ActPurchase.Services.Contracts.DTO;

/// <summary>
/// DTO для создания. Это то, что клиент присылает серверу
/// </summary>
public class ActItemCreateModel
{
    /// <summary>
    /// Идентификатор акта
    /// </summary>
    public Guid ActId { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Количество
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Цена за единицу
    /// </summary>
    public decimal Price { get; set; }
}
