using ActPurchase.DataAccess.Dal.Contracts.Interfaces;

namespace ActPurchase.DataAccess.Dal.Contracts;

/// <summary>
/// Базовый класс с аудитом
/// </summary>
public abstract class BaseAuditEntity :
    IEntityWithId,
    IEntityAuditCreated,
    IEntityAuditUpdate,
    IEntityAuditDeletedAt
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTimeOffset UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTimeOffset? DeletedAt { get; set; }
}
