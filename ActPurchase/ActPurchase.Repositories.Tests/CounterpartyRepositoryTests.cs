using FluentAssertions;
using Xunit;
using ActPurchase.Context.Tests;
using ActPurchase.Domain.Entites;
using ActPurchase.Domain.Enum;
using ActPurchase.Repositories.Contracts;
using Ahatornn.TestGenerator;

namespace ActPurchase.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="CounterpartyRepository"/>
/// </summary>
public class CounterpartyRepositoryTests : ActPurchaseContextInMemory
{
    private readonly ICounterpartyRepository repository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CounterpartyRepositoryTests"/>
    /// </summary>
    public CounterpartyRepositoryTests()
    {
        repository = new CounterpartyRepository(WriterContext, Context);
    }

    /// <summary>
    /// возвращает пустую коллекцию, если БД пустая
    /// </summary>
    [Fact]
    public async Task GetAllCounterpartiesShouldReturnEmpty()
    {
        // Act
        var items = await repository.GetAllCounterpartiesAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// возвращает неудалённые контрагенты, отсортированные по названию
    /// </summary>
    [Fact]
    public async Task GetAllCounterpartiesShouldReturnValue()
    {
        // Arrange
        var item1 = TestEntityProvider.Shared.Create<Counterparty>(x => x.CompanyName = "Компания1");
        var item2 = TestEntityProvider.Shared.Create<Counterparty>(x =>
        {
            x.CompanyName = "Компания2";
            x.DeletedAt = DateTimeOffset.Now;
        });
        var item3 = TestEntityProvider.Shared.Create<Counterparty>(x => x.CompanyName = "Компания3");

        Context.AddRange(item1, item2, item3);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetAllCounterpartiesAsync(CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.HaveCount(2)
            .And.BeInAscendingOrder(x => x.CompanyName)
            .And.ContainSingle(x => x.Id == item1.Id)
            .And.ContainSingle(x => x.Id == item3.Id);
    }

    /// <summary>
    /// возвращает контрагента по идентификатору
    /// </summary>
    [Fact]
    public async Task GetCounterpartyByIdShouldReturnValue()
    {
        // Arrange
        var item = TestEntityProvider.Shared.Create<Counterparty>();
        Context.Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetCounterpartyByIdAsync(item.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(item.Id);
    }

    /// <summary>
    /// возвращает null, если контрагент не найден
    /// </summary>
    [Fact]
    public async Task GetCounterpartyByIdShouldReturnNull()
    {
        // Act
        var result = await repository.GetCounterpartyByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// возвращает null, если контрагент удалён
    /// </summary>
    [Fact]
    public async Task GetCounterpartyByIdShouldReturnNullForDeleted()
    {
        // Arrange
        var item = TestEntityProvider.Shared.Create<Counterparty>(x => x.DeletedAt = DateTimeOffset.Now);
        Context.Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetCounterpartyByIdAsync(item.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// возвращает контрагента по ИНН
    /// </summary>
    [Fact]
    public async Task GetCounterpartyByInnShouldReturnValue()
    {
        // Arrange
        var item = TestEntityProvider.Shared.Create<Counterparty>(x => x.Inn = "1234567890");
        Context.Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetCounterpartyByInnAsync("1234567890", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(item.Id);
    }

    /// <summary>
    /// возвращает null, если контрагент не найден
    /// </summary>
    [Fact]
    public async Task GetCounterpartyByInnShouldReturnNull()
    {
        // Act
        var result = await repository.GetCounterpartyByInnAsync("9999999999", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// возвращает null, если контрагент удалён
    /// </summary>
    [Fact]
    public async Task GetCounterpartyByInnShouldReturnNullForDeleted()
    {
        // Arrange
        var item = TestEntityProvider.Shared.Create<Counterparty>(x =>
        {
            x.Inn = "1234567890";
            x.DeletedAt = DateTimeOffset.Now;
        });

        Context.Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetCounterpartyByInnAsync("1234567890", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// возвращает контрагентов указанного типа
    /// </summary>
    [Fact]
    public async Task GetCounterpartiesByTypeShouldReturnValue()
    {
        // Arrange
        var buyer1 = TestEntityProvider.Shared.Create<Counterparty>(x => x.Type = CounterpartyType.Buyer);
        var buyer2 = TestEntityProvider.Shared.Create<Counterparty>(x => x.Type = CounterpartyType.Buyer);
        var seller1 = TestEntityProvider.Shared.Create<Counterparty>(x => x.Type = CounterpartyType.Seller);

        Context.AddRange(buyer1, buyer2, seller1);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetCounterpartiesByTypeAsync(CounterpartyType.Buyer, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.HaveCount(2)
            .And.OnlyContain(x => x.Type == CounterpartyType.Buyer);
    }

    /// <summary>
    /// возвращает пустую коллекцию, если контрагентов такого типа нет
    /// </summary>
    [Fact]
    public async Task GetCounterpartiesByTypeShouldReturnEmpty()
    {
        // Arrange
        var seller = TestEntityProvider.Shared.Create<Counterparty>(x => x.Type = CounterpartyType.Seller);
        Context.Add(seller);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetCounterpartiesByTypeAsync(CounterpartyType.Buyer, CancellationToken.None);

        // Assert
        result.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// возвращает только покупателей
    /// </summary>
    [Fact]
    public async Task GetBuyersShouldReturnOnlyBuyers()
    {
        // Arrange
        var buyer1 = TestEntityProvider.Shared.Create<Counterparty>(x => x.Type = CounterpartyType.Buyer);
        var buyer2 = TestEntityProvider.Shared.Create<Counterparty>(x => x.Type = CounterpartyType.Buyer);
        var seller = TestEntityProvider.Shared.Create<Counterparty>(x => x.Type = CounterpartyType.Seller);

        Context.AddRange(buyer1, buyer2, seller);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetBuyersAsync(CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.HaveCount(2)
            .And.OnlyContain(x => x.Type == CounterpartyType.Buyer);
    }

    /// <summary>
    /// возвращает только продавцов
    /// </summary>
    [Fact]
    public async Task GetSellersShouldReturnOnlySellers()
    {
        // Arrange
        var seller1 = TestEntityProvider.Shared.Create<Counterparty>(x => x.Type = CounterpartyType.Seller);
        var seller2 = TestEntityProvider.Shared.Create<Counterparty>(x => x.Type = CounterpartyType.Seller);
        var buyer = TestEntityProvider.Shared.Create<Counterparty>(x => x.Type = CounterpartyType.Buyer);

        Context.AddRange(seller1, seller2, buyer);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetSellersAsync(CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.HaveCount(2)
            .And.OnlyContain(x => x.Type == CounterpartyType.Seller);
    }
}
