namespace ActPurchase.Domain.Enum;

/// <summary>
/// Тип контрагента, покупатель или продавец, тк у них одинаковые св-ва
/// </summary>
public enum CounterpartyType
{
    /// <summary>
    /// Покупатель
    /// </summary>
    Buyer = 0,

    /// <summary>
    /// Продавец
    /// </summary>
    Seller = 1
}
