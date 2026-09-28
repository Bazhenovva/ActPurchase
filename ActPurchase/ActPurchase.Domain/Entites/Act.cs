namespace ActPurchase.Domain.Entities;

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
    /// Дата составления (число месяц год)
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Тип акта, его название
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Внешний ключ: идентификатор покупателя
    /// </summary>
    public Guid BuyerId { get; set; }

    /// <summary>
    /// Навигационное свойство: покупатель
    /// </summary>
    public Counterparty? Buyer { get; set; }

    /// <summary>
    /// Внешний ключ: идентификатор продавца
    /// </summary>
    public Guid SellerId { get; set; }

    /// <summary>
    /// Навигационное свойство: продавец
    /// </summary>
    public Counterparty? Seller { get; set; }

    /// <summary>
    /// Внешний ключ: идентификатор директора
    /// </summary>
    public Guid DirectorId { get; set; }

    /// <summary>
    /// Навигационное свойство: директор
    /// </summary>
    public Director? Director { get; set; }

    /// <summary>
    /// Итоговое количество по всем позициям
    /// </summary>
    public int TotalQuantity { get; set; }

    /// <summary>
    /// Итоговая сумма по всем позициям
    /// </summary>
    public decimal TotalSum { get; set; }

    /// <summary>
    /// Навигационное свойство: позиции акта
    /// </summary>
    public List<ActItem> Items { get; set; } = new();
}
