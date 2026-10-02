using ActPurchase.Context.Tests;
using ActPurchase.Domain.Entites;
using ActPurchase.Repositories;
using ActPurchase.Services.Automapper;
using ActPurchase.Services.Contracts.DTO;
using ActPurchase.Services.Contracts.Exceptions;
using ActPurchase.Services.Contracts.Interfaces;
using ActPurchase.Services.Services;
using Ahatornn.TestGenerator;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ActPurchase.Services.Tests;

/// <summary>
/// Тесты для <see cref="CounterpartyService"/>
/// </summary>
public class CounterpartyServiceTests : ActPurchaseContextInMemory
{
    private readonly ICounterpartyService counterpartyService;

    /// <summary>
    /// Инициализирует тестовый класс
    /// </summary>
    public CounterpartyServiceTests()
    {
        var repository = new CounterpartyRepository(WriterContext, Context);

        var profile = new ServiceProfile();
        var mapper = new MapperConfiguration(x => x.AddProfile(profile), NullLoggerFactory.Instance).CreateMapper();

        counterpartyService = new CounterpartyService(repository, UnitOfWork, mapper);
    }

    /// <summary>
    /// Пустой список контрагентов
    /// </summary>
    [Fact]
    public async Task GetAllCounterpartiesShouldReturnEmpty()
    {
        // Act
        var items = await counterpartyService.GetAllCounterpartiesAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// Список контрагентов без удалённых
    /// </summary>
    [Fact]
    public async Task GetAllCounterpartiesShouldReturnValues()
    {
        // Arrange
        var item1 = TestEntityProvider.Shared.Create<Counterparty>();
        var item2 = TestEntityProvider.Shared.Create<Counterparty>(x => x.DeletedAt = DateTimeOffset.Now);
        var item3 = TestEntityProvider.Shared.Create<Counterparty>();
        Context.AddRange(item1, item2, item3);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var items = await counterpartyService.GetAllCounterpartiesAsync(CancellationToken.None);

        // Assert
        items.Should()
            .NotBeNull()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == item1.Id)
            .And.ContainSingle(x => x.Id == item3.Id);
    }

    /// <summary>
    /// Получение контрагента по идентификатору
    /// </summary>
    [Fact]
    public async Task GetCounterpartyByIdShouldReturnValue()
    {
        // Arrange
        var entity = TestEntityProvider.Shared.Create<Counterparty>();
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await counterpartyService.GetCounterpartyByIdAsync(entity.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.CompanyName.Should().Be(entity.CompanyName);
        result.Inn.Should().Be(entity.Inn);
    }

    /// <summary>
    /// Исключение при несуществующем идентификаторе
    /// </summary>
    [Fact]
    public async Task GetCounterpartyByIdShouldThrowNotFoundException()
    {
        // Act
        var act = () => counterpartyService.GetCounterpartyByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Counterparty>>();
    }

    /// <summary>
    /// Успешное создание контрагента
    /// </summary>
    [Fact]
    public async Task CreateCounterpartyShouldWork()
    {
        // Arrange
        var createModel = TestEntityProvider.Shared.Create<CounterpartyCreateModel>();

        // Act
        await counterpartyService.CreateCounterpartyAsync(createModel, CancellationToken.None);

        // Assert
        Context.Set<Counterparty>().Should().BeEquivalentTo([new
        {
            Type = createModel.Type,
            CompanyName = createModel.CompanyName,
            Inn = createModel.Inn,
            Kpp = createModel.Kpp,
            Ogrn = createModel.Ogrn,
            CheckingAccount = createModel.CheckingAccount,
            CorrespondentAccount = createModel.CorrespondentAccount,
            Bik = createModel.Bik,
            BankName = createModel.BankName,
            LegalAddress = createModel.LegalAddress
        }]);
    }

    /// <summary>
    /// Исключение при дубликате ИНН
    /// </summary>
    [Fact]
    public async Task CreateCounterpartyShouldThrowExceptionIfInnExists()
    {
        // Arrange
        var existing = TestEntityProvider.Shared.Create<Counterparty>(x => x.Inn = "1234567890");
        Context.Add(existing);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var createModel = TestEntityProvider.Shared.Create<CounterpartyCreateModel>(x => x.Inn = "1234567890");

        // Act
        var act = () => counterpartyService.CreateCounterpartyAsync(createModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ActPurchaseException>();
    }

    /// <summary>
    /// Успешное обновление контрагента
    /// </summary>
    [Fact]
    public async Task UpdateCounterpartyShouldWork()
    {
        // Arrange
        var entity = TestEntityProvider.Shared.Create<Counterparty>();
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var updateModel = TestEntityProvider.Shared.Create<CounterpartyModel>(x =>
        {
            x.Id = entity.Id;
            x.CompanyName = "Новая компания";
        });

        // Act
        await counterpartyService.UpdateCounterpartyAsync(updateModel, CancellationToken.None);

        // Assert
        Context.Set<Counterparty>().Should().BeEquivalentTo([new
        {
            CompanyName = updateModel.CompanyName,
            Inn = updateModel.Inn,
            Kpp = updateModel.Kpp
        }]);
    }

    /// <summary>
    /// Исключение при несуществующем идентификаторе
    /// </summary>
    [Fact]
    public async Task UpdateCounterpartyShouldThrowNotFoundException()
    {
        // Arrange
        var updateModel = TestEntityProvider.Shared.Create<CounterpartyModel>(x => x.Id = Guid.NewGuid());

        // Act
        var act = () => counterpartyService.UpdateCounterpartyAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Counterparty>>();
    }

    /// <summary>
    /// Исключение при дубликате ИНН у другого контрагента
    /// </summary>
    [Fact]
    public async Task UpdateCounterpartyShouldThrowExceptionIfInnExists()
    {
        // Arrange
        var entity = TestEntityProvider.Shared.Create<Counterparty>(x => x.Inn = "1111111111");
        var other = TestEntityProvider.Shared.Create<Counterparty>(x => x.Inn = "2222222222");
        Context.AddRange(entity, other);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var updateModel = TestEntityProvider.Shared.Create<CounterpartyModel>(x =>
        {
            x.Id = entity.Id;
            x.Inn = "2222222222";
        });

        // Act
        var act = () => counterpartyService.UpdateCounterpartyAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ActPurchaseException>();
    }

    /// <summary>
    /// Успешное удаление контрагента
    /// </summary>
    [Fact]
    public async Task DeleteCounterpartyShouldWork()
    {
        // Arrange
        var entity = TestEntityProvider.Shared.Create<Counterparty>();
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        await counterpartyService.DeleteCounterpartyAsync(entity.Id, CancellationToken.None);

        // Assert
        Context.Set<Counterparty>().FirstOrDefault()?.DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Исключение при несуществующем идентификаторе
    /// </summary>
    [Fact]
    public async Task DeleteCounterpartyShouldThrowNotFoundException()
    {
        // Act
        var act = () => counterpartyService.DeleteCounterpartyAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Counterparty>>();
    }
}
