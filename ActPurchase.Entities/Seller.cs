using ActPurchase.DataAccess.Dal.Contracts;

namespace ActPurchase.Entities;

/// <summary>
/// Продавец 
/// </summary>
public class Seller : BaseAuditEntity
{
    /// <summary>
    /// ФИО
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Серия паспорта
    /// </summary>
    public string PassportSeries { get; set; } = string.Empty;

    /// <summary>
    /// Номер паспорта
    /// </summary>
    public string PassportNumber { get; set; } = string.Empty;

    /// <summary>
    /// Кем выдан
    /// </summary>
    public string IssuedBy { get; set; } = string.Empty;

    /// <summary>
    /// Дата выдачи паспорта
    /// </summary>
    public DateOnly IssueDate { get; set; }

    /// <summary>
    /// Адрес регистрации
    /// </summary>
    public string Address { get; set; } = string.Empty;
}
