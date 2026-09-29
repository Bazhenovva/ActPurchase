using ActPurchase.Domain.Interfaces;

namespace ActPurchase.Domain;

/// <summary>
/// Базовый класс, который наследуется во всех сущностях,чтобы не повторять код.
/// Реализует все интерфейсы аудита.
/// </summary>
public abstract class BaseAuditEntity :
    IEntity,
    IEntityWithId,
    IEntityAuditCreated,
    IEntityAuditUpdated,
    IEntityAuditDeletedAt
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Кем создано
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Дата обновления
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    ///Кем обновлено
    /// </summary>
    public string UpdatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Дата удаления
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    /// <summary>
    /// Кем удалено
    /// </summary>
    public string? DeletedBy { get; set; }
}
