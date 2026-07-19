using ActPurchase.DataAccess.Dal.Contracts;

namespace ActPurchase.Entities;

/// <summary>
/// Акт закупки имущества
/// </summary>
public class Act : BaseAuditEntity
{
    /// <summary>
    /// Номер акта
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Дата составления
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Город составления
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Итоговая сумма
    /// </summary>
    public decimal TotalAmount { get; set; }
}
