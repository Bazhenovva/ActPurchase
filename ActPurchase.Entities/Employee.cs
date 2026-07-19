using ActPurchase.DataAccess.Dal.Contracts;

namespace ActPurchase.Entities;

/// <summary>
/// Сотрудник покупателя
/// </summary>
public class Employee : BaseAuditEntity
{
    /// <summary>
    /// ФИО
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Должность
    /// </summary>
    public string Position { get; set; } = string.Empty;

    /// <summary>
    /// Номер доверенности
    /// </summary>
    public string PowerOfAttorneyNumber { get; set; } = string.Empty;

    /// <summary>
    /// Дата доверенности
    /// </summary>
    public DateOnly PowerOfAttorneyDate { get; set; }
}
