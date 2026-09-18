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
    /// Покупатель
    /// </summary>
    public Counterparty? Buyer { get; set; }

    /// <summary>
    /// Идентификатор продавца
    /// </summary>
    public Guid SellerId { get; set; }

    /// <summary>
    /// Продавец
    /// </summary>
    public Counterparty? Seller { get; set; }

    /// <summary>
    /// Идентификатор директора
    /// </summary>
    public Guid DirectorId { get; set; }

    /// <summary>
    /// Директор
    /// </summary>
    public Director? Director { get; set; }

    /// <summary>
    /// Итоговое количество
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// Итоговая цена
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Итоговая сумма
    /// </summary>
    public decimal TotalSum { get; set; }

    /// <summary>
    /// Позиции акта
    /// </summary>
    public List<ActItem> Items { get; set; } = new();
}
