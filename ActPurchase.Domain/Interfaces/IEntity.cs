namespace ActPurchase.Domain.Interfaces;

/// <summary>
/// Базовая сущность с идентификатором
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    Guid Id { get; set; }
}
