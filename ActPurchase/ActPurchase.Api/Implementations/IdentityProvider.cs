using ActPurchase.Common;

namespace ActPurchase.Api.Implementations;

/// <summary>
///  Возвращает имя текущего пользователя
/// </summary>
public class IdentityProvider : IIdentityProvider
{
    /// <summary>
    /// Имя текущего пользователя
    /// </summary>
    public string Name => "Admin";
}
