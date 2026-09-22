namespace ActPurchase.Domain.Entities;

/// <summary>
/// Позиция акта закупки
/// </summary>
public class ActItem : BaseAuditEntity
{
    /// <summary>
    /// Идентификатор акта, внешний ключ
    /// </summary>
    public Guid ActId { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Количество товара
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Цена за единицу
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Сумма
    /// </summary>
    public decimal Sum { get; set; }
}
