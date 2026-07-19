namespace ActPurchase.DataAccess.Dal.Contracts.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IEntityAuditCreated
{
    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string CreatedBy { get; set; }
}
