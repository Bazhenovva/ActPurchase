using ActPurchase.Context;
using ActPurchase.Dal.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ActPurchase.Context.Tests;

/// <summary>
/// Контекст <see cref="ActPurchaseContext"/> для тестов с базой в памяти. Один контекст на тест.
/// </summary>
public class ActPurchaseContextInMemory : IAsyncDisposable
{
    /// <summary>
    /// Контекст <see cref="ActPurchaseContext"/>
    /// </summary>
    protected ActPurchaseContext Context { get; }

    /// <summary>
    /// Единица работы для сохранения изменений 
    /// </summary>
    protected IUnitOfWork UnitOfWork => Context;

    /// <summary>
    /// Тестовый контекст записи с моками провайдеров
    /// </summary>
    protected IDbWriterContext WriterContext => new TestWriterContext(Context);

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ActPurchaseContextInMemory"/>
    /// </summary>
    protected ActPurchaseContextInMemory()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ActPurchaseContext>()
            .UseInMemoryDatabase($"ActPurchaseContextTests{Guid.NewGuid()}")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));

        Context = new ActPurchaseContext(optionsBuilder.Options);
    }

    /// <summary>
    /// Асинхронно освобождает ресурсы
    /// </summary>
    /// <returns></returns>
    public async ValueTask DisposeAsync()
    {
        await Context.Database.EnsureDeletedAsync();
        await Context.DisposeAsync();
    }
}
