namespace ActPurchase.DataAccess.Dal.Contracts.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IEntityAuditUpdate
{
    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string UpdatedBy { get; set; }
}
