using ActPurchase.DataAccess.Dal.Contracts;

namespace ActPurchase.Entities;

/// <summary>
/// Директор организации
/// </summary>
public class Director : BaseAuditEntity
{
    /// <summary>
    /// Фамилия
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Отчество
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// Название организации
    /// </summary>
    public string CompanyName { get; set; } = string.Empty;
}
