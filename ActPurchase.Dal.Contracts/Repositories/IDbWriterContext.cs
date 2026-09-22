using ActPurchase.Common;

namespace ActPurchase.Dal.Contracts.Repositories
{
    /// <summary>
    /// контракт, который объединяет всё для записи с аудитом
    /// </summary>
    public interface IDbWriterContext
    {
        /// <summary>
        /// Writer для записи сущностей»
        /// </summary>
        IWriter Writer { get; }

        /// <summary>
        /// Провайдер текущего времени
        /// </summary>
        IDateTimeProvider DateTimeProvider { get; }

        /// <summary>
        /// Провайдер имени текда ущего пользователя
        /// </summary>
        IIdentityProvider IdentityProvider { get; }

    }
}
