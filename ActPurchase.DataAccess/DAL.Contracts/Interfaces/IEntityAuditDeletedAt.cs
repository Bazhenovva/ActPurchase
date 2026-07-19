namespace ActPurchase.DataAccess.Dal.Contracts.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IEntityAuditDeletedAt
{
    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }
}
