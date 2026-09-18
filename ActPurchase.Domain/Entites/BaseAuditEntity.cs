using ActPurchase.Domain.Interfaces;

namespace ActPurchase.Domain.Entities;

/// <summary>
/// Базовый класс, который наследуется во всех сущностях,чтобы не повторять код.
/// Реализует все интерфейсы аудита.
/// </summary>
public abstract class BaseAuditEntity :
    IEntity,
    IEntityWithId,
    IEntityAuditCreated,
    IEntityAuditUpdate,
    IEntityAuditDeletedAt
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string UpdatedBy { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? DeletedBy { get; set; }
}
