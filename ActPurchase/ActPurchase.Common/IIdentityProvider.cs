namespace ActPurchase.Common
{
    /// <summary>
    /// Контракт для получения имени текущего пользователя
    /// </summary>
    public interface IIdentityProvider
    {
        /// <summary>
        /// Возвращает имя текущего пользователя
        /// </summary>
        string Name { get; }
    }
}
