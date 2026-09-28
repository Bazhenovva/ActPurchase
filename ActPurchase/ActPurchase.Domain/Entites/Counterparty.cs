using ActPurchase.Domain.Enum;

namespace ActPurchase.Domain.Entities;

/// <summary>
/// Контрагент ,покупатель или продавец
/// </summary>
public class Counterparty : BaseAuditEntity
{
    /// <summary>
    /// Тип контрагента, покупатель или продавец
    /// </summary>
    public CounterpartyType Type { get; set; }

    /// <summary>
    /// Название кампании
    /// </summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// ИНН
    /// </summary>
    public string Inn { get; set; } = string.Empty;

    /// <summary>
    /// КПП
    /// </summary>
    public string Kpp { get; set; } = string.Empty;

    /// <summary>
    /// ОГРН
    /// </summary>
    public string Ogrn { get; set; } = string.Empty;

    /// <summary>
    /// Расчётный счёт
    /// </summary>
    public string CheckingAccount { get; set; } = string.Empty;

    /// <summary>
    /// Корреспондентский счёт
    /// </summary>
    public string CorrespondentAccount { get; set; } = string.Empty;

    /// <summary>
    /// БИК
    /// </summary>
    public string Bik { get; set; } = string.Empty;

    /// <summary>
    /// Название банка
    /// </summary>
    public string BankName { get; set; } = string.Empty;

    /// <summary>
    /// Юридический адрес
    /// </summary>
    public string LegalAddress { get; set; } = string.Empty;
}
