using ActPurchase.Context;
using ActPurchase.Dal.Contracts.Repositories;

namespace ActPurchase.Api.Tests.Infrastructure
{
    /// <summary>
    /// Базовый класс для интеграционных тестов
    /// Предоставляет доступ к БД, UnitOfWork и HTTP-клиенту
    /// </summary>
    [Collection(nameof(ActPurchaseApiTestCollection))]
    public abstract class IntegrationTestBase
    {
        private readonly ActPurchaseApiFixture fixture;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="IntegrationTestBase"/>
        /// </summary>
        protected IntegrationTestBase(ActPurchaseApiFixture fixture)
        {
            this.fixture = fixture;
            Context = fixture.Context;
            UnitOfWork = fixture.UnitOfWork;
        }

        /// <summary>
        /// Контекст БД из DI-контейнера
        /// </summary>
        protected ActPurchaseContext Context { get; }

        /// <summary>
        /// UnitOfWork для сохранения изменений
        /// </summary>
        protected IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Создаёт HTTP-клиент для запросов к API
        /// </summary>
        protected HttpClient CreateClient() => fixture.CreateClient();
    }
}
