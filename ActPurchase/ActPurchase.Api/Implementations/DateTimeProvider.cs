using ActPurchase.Common;

namespace ActPurchase.Api.Implementations;

/// <summary>
/// Провайдер текущего времени
/// </summary>
public class DateTimeProvider : IDateTimeProvider
{
    /// <summary>
    /// Текущее время
    /// </summary>
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
