namespace ActPurchase.Domain.Interfaces;

/// <summary>
/// Сущность с аудитом создания
/// </summary>
public interface IEntityAuditCreated
{
    /// <summary>
    /// Дата создания
    /// </summary>
    DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Кем создано
    /// </summary>
    string CreatedBy { get; set; }
}
