using ActPurchase.Common;
using ActPurchase.Dal.Contracts.Repositories;
using Moq;

namespace ActPurchase.Context.Tests;

/// <summary>
/// Тестовая реализация <see cref="IDbWriterContext"/> с моками провайдеров
/// </summary>
public class TestWriterContext : IDbWriterContext
{
    /// <summary>
    /// Мок провайдера времени
    /// </summary>
    private readonly Mock<IDateTimeProvider> dateTimeProviderMock;

    /// <summary>
    /// Мок провайдера идентификации пользователя
    /// </summary>
    private readonly Mock<IIdentityProvider> identityProviderMock;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="TestWriterContext"/>
    /// </summary>
    public TestWriterContext(IWriter writer)
    {
        Writer = writer;

        dateTimeProviderMock = new Mock<IDateTimeProvider>();
        dateTimeProviderMock.Setup(x => x.UtcNow).Returns(DateTimeOffset.UtcNow);

        identityProviderMock = new Mock<IIdentityProvider>();
        identityProviderMock.Setup(x => x.Name).Returns("test@test-identity");
    }

    /// <summary>
    /// Writer для записи сущностей»
    /// </summary>
    public IWriter Writer { get; }

    /// <summary>
    /// Провайдер текущего времени
    /// </summary>
    public IDateTimeProvider DateTimeProvider => dateTimeProviderMock.Object;

    /// <summary>
    /// Провайдер имени текда ущего пользователя
    /// </summary>
    public IIdentityProvider IdentityProvider => identityProviderMock.Object;
}
