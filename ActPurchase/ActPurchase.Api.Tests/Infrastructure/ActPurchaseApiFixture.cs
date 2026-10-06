using ActPurchase.Context;
using ActPurchase.Dal.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ActPurchase.Api.Tests.Infrastructure
{
    /// <summary>
    ///  Создаёт фабрику один раз, применяет миграции к тестовой БД и удаляет её после тестов
    /// </summary>
    public class ActPurchaseApiFixture : IAsyncLifetime
    {
        private bool disposed;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ActPurchaseApiFixture"/>
        /// </summary>
        public ActPurchaseApiFixture()
        {
            Factory = new TestWebApplicationFactory();
        }

        /// <summary>
        /// Фабрика тестового веб-приложения
        /// </summary>
        protected TestWebApplicationFactory Factory { get; }

        /// <summary>
        /// Контекст БД из DI-контейнера
        /// </summary>
        internal ActPurchaseContext Context
        {
            get
            {
                if (field != null)
                {
                    return field;
                }
                var scope = Factory.Services.GetRequiredService<IServiceScopeFactory>()
                    .CreateScope();
                field =  scope.ServiceProvider.GetRequiredService<ActPurchaseContext>();
                return field;
            }
        }

        /// <summary>
        /// UnitOfWork для сохранения изменений
        /// </summary>
        internal IUnitOfWork UnitOfWork => Context;

        /// <summary>
        ///  Провайдер сервисов DI-контейнера
        /// </summary>
        internal IServiceProvider Services => Factory.Services;

        /// <summary>
        /// Создаёт HTTP-клиент для запросов к API
        /// </summary>
        internal HttpClient CreateClient() => Factory.CreateClient();

        /// <summary>
        /// Применяет миграции к тестовой БД перед тестами
        /// </summary>
        public virtual async ValueTask InitializeAsync()
        {
            await Context.Database.MigrateAsync();
        }

        /// <summary>
        /// Удаляет тестовую БД и освобождает ресурсы после тестов
        /// </summary>
        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;
            await Context.Database.EnsureDeletedAsync();
            await Context.Database.CloseConnectionAsync();
            await Context.DisposeAsync();
            await Factory.DisposeAsync();
        }
    }
}
