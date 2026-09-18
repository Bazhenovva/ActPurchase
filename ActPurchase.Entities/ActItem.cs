using ActPurchase.DataAccess.Dal.Contracts;

namespace ActPurchase.Entities;

/// <summary>
/// Позиция акта закупки
/// </summary>
public class ActItem : BaseAuditEntity
{
    /// <summary>
    /// Идентификатор акта
    /// </summary>
    public Guid ActId { get; set; }

    /// <summary>
    /// Акт
    /// </summary>
    public Act? Act { get; set; }

    /// <summary>
    /// Номер по порядку
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Единица измерения
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Количество
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
