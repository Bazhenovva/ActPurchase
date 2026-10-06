namespace ActPurchase.Api.Tests.Infrastructure;

/// <summary>
/// Коллекция для интеграционных тестов.
/// Все тесты коллекции используют одну фикстуру <see cref="ActPurchaseApiFixture"/>.
/// </summary>
[CollectionDefinition(nameof(ActPurchaseApiTestCollection))]
public class ActPurchaseApiTestCollection : ICollectionFixture<ActPurchaseApiFixture>;
