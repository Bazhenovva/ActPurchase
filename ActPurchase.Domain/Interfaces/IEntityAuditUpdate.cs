namespace ActPurchase.Domain.Interfaces;

/// <summary>
/// Сущность с аудитом обновления
/// </summary>
public interface IEntityAuditUpdate
{
    /// <summary>
    /// Дата обновления
    /// </summary>
    DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// Кем обновлено
    /// </summary>
    string UpdatedBy { get; set; }
}
