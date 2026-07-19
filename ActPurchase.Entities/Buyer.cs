using ActPurchase.DataAccess.Dal.Contracts;

namespace ActPurchase.Entities;

/// <summary>
/// Покупатель
/// </summary>
public class Buyer : BaseAuditEntity
{
    /// <summary>
    /// Наименование организации
    /// </summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// ИНН
    /// </summary>
    public string Inn { get; set; } = string.Empty;

    /// <summary>
    /// КПП - это код причины постановки на учёт
    /// </summary>
    public string Kpp { get; set; } = string.Empty;

    /// <summary>
    /// ОГРН
    /// </summary>
    public string Ogrn { get; set; } = string.Empty;

    /// <summary>
    /// Наименование банка
    /// </summary>
    public string BankName { get; set; } = string.Empty;

    /// <summary>
    /// Расчетный счет
    /// </summary>
    public string BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// Корреспондентский счет
    /// </summary>
    public string CorrespondentAccount { get; set; } = string.Empty;

    /// <summary>
    /// БИК - банковский идентификационный код
    /// </summary>
    public string Bik { get; set; } = string.Empty;

    /// <summary>
    /// Юридический адрес
    /// </summary>
    public string LegalAddress { get; set; } = string.Empty;
}
