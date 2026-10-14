using ActPurchase.Common;
using ActPurchase.Dal.Contracts.Repositories;

namespace ActPurchase.Api.Implementations;

/// <summary>
/// Объединяет writer, провайдер времени и провайдер пользователя для репозиториев
/// </summary>
public class DbWriterContext : IDbWriterContext
{
    /// <summary>
    /// Создаёт контекст
    /// </summary>
    public DbWriterContext(IWriter writer, IDateTimeProvider dateTimeProvider, IIdentityProvider identityProvider)
    {
        Writer = writer;
        DateTimeProvider = dateTimeProvider;
        IdentityProvider = identityProvider;
    }

    /// <summary>
    /// Writer для сохранения сущностей
    /// </summary>
    public IWriter Writer { get; }

    /// <summary>
    /// Провайдер текущего времени
    /// </summary>
    public IDateTimeProvider DateTimeProvider { get; }

    /// <summary>
    /// Провайдер имени пользователя
    /// </summary>
    public IIdentityProvider IdentityProvider { get; }
}
