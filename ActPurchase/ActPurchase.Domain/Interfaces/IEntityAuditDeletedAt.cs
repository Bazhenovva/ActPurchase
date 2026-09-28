namespace ActPurchase.Domain.Interfaces;

/// <summary>
/// Сущность с аудитом удаления 
/// </summary>
public interface IEntityAuditDeletedAt
{
    /// <summary>
    /// Дата удаления
    /// </summary>
    DateTimeOffset? DeletedAt { get; set; }

    /// <summary>
    /// Кем удалено
    /// </summary>
    string? DeletedBy { get; set; }
}
