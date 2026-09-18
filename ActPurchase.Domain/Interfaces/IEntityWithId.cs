namespace ActPurchase.Domain.Interfaces;

/// <summary>
/// сущность с идентификатором, он используется для настройки ключа в конфигурациях EF Core
/// в методе"HasIdAsKey
/// </summary>
public interface IEntityWithId
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    Guid Id { get; set; }
}
