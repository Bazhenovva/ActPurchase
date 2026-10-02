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
/// Тесты для <see cref="ActService"/>
/// </summary>
public class ActServiceTests : ActPurchaseContextInMemory
{
    private readonly IActService actService;

    /// <summary>
    /// Инициализирует тестовый класс
    /// </summary>
    public ActServiceTests()
    {
        var repository = new ActRepository(WriterContext, Context);

        var profile = new ServiceProfile();
        var mapper = new MapperConfiguration(x => x.AddProfile(profile), NullLoggerFactory.Instance).CreateMapper();

        actService = new ActService(repository, UnitOfWork, mapper);
    }

    /// <summary>
    /// Пустой список актов
    /// </summary>
    [Fact]
    public async Task GetAllActsShouldReturnEmpty()
    {
        // Act
        var items = await actService.GetAllActsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// Список актов без удалённых
    /// </summary>
    [Fact]
    public async Task GetAllActsShouldReturnValues()
    {
        // Arrange
        var item1 = TestEntityProvider.Shared.Create<Act>(x => x.Number = "001");
        var item2 = TestEntityProvider.Shared.Create<Act>(x => x.DeletedAt = DateTimeOffset.Now);
        var item3 = TestEntityProvider.Shared.Create<Act>(x => x.Number = "002");
        Context.AddRange(item1, item2, item3);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var items = await actService.GetAllActsAsync(CancellationToken.None);

        // Assert
        items.Should()
            .NotBeNull()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == item1.Id)
            .And.ContainSingle(x => x.Id == item3.Id);
    }

    /// <summary>
    /// Получение акта по идентификатору
    /// </summary>
    [Fact]
    public async Task GetByIdShouldReturnValue()
    {
        // Arrange
        var entity = TestEntityProvider.Shared.Create<Act>();
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await actService.GetByIdAsync(entity.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.Number.Should().Be(entity.Number);
        result.City.Should().Be(entity.City);
    }

    /// <summary>
    /// Исключение при несуществующем идентификаторе
    /// </summary>
    [Fact]
    public async Task GetByIdShouldThrowNotFoundException()
    {
        // Act
        var act = () => actService.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Act>>();
    }

    /// <summary>
    /// Успешное создание акта
    /// </summary>
    [Fact]
    public async Task CreateActShouldWork()
    {
        // Arrange
        var createModel = TestEntityProvider.Shared.Create<ActCreateModel>();

        // Act
        await actService.CreateActAsync(createModel, CancellationToken.None);

        // Assert
        Context.Set<Act>().Should().BeEquivalentTo([new
        {
            Number = createModel.Number,
            City = createModel.City,
            Type = createModel.Type,
            BuyerId = createModel.BuyerId,
            SellerId = createModel.SellerId,
            DirectorId = createModel.DirectorId
        }]);
    }

    /// <summary>
    /// Исключение при дубликате номера
    /// </summary>
    [Fact]
    public async Task CreateActShouldThrowExceptionIfNumberExists()
    {
        // Arrange
        var existing = TestEntityProvider.Shared.Create<Act>(x => x.Number = "001");
        Context.Add(existing);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var createModel = TestEntityProvider.Shared.Create<ActCreateModel>(x => x.Number = "001");

        // Act
        var act = () => actService.CreateActAsync(createModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ActPurchaseException>();
    }

    /// <summary>
    /// Успешное обновление акта
    /// </summary>
    [Fact]
    public async Task UpdateActShouldWork()
    {
        // Arrange
        var entity = TestEntityProvider.Shared.Create<Act>();
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var updateModel = TestEntityProvider.Shared.Create<ActModel>(x =>
        {
            x.Id = entity.Id;
            x.Number = "999";
        });

        // Act
        await actService.UpdateActAsync(updateModel, CancellationToken.None);

        // Assert
        Context.Set<Act>().Should().BeEquivalentTo([new
        {
            Number = updateModel.Number,
            City = updateModel.City,
            Type = updateModel.Type
        }]);
    }

    /// <summary>
    /// Исключение при несуществующем идентификаторе
    /// </summary>
    [Fact]
    public async Task UpdateActShouldThrowNotFoundException()
    {
        // Arrange
        var updateModel = TestEntityProvider.Shared.Create<ActModel>(x => x.Id = Guid.NewGuid());

        // Act
        var act = () => actService.UpdateActAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Act>>();
    }

    /// <summary>
    /// Исключение при дубликате номера у другого акта
    /// </summary>
    [Fact]
    public async Task UpdateActShouldThrowExceptionIfNumberExists()
    {
        // Arrange
        var entity = TestEntityProvider.Shared.Create<Act>(x => x.Number = "001");
        var other = TestEntityProvider.Shared.Create<Act>(x => x.Number = "002");
        Context.AddRange(entity, other);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var updateModel = TestEntityProvider.Shared.Create<ActModel>(x =>
        {
            x.Id = entity.Id;
            x.Number = "002";
        });

        // Act
        var act = () => actService.UpdateActAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ActPurchaseException>();
    }

    /// <summary>
    /// Успешное удаление акта
    /// </summary>
    [Fact]
    public async Task DeleteActShouldWork()
    {
        // Arrange
        var entity = TestEntityProvider.Shared.Create<Act>();
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        await actService.DeleteActAsync(entity.Id, CancellationToken.None);

        // Assert
        Context.Set<Act>().FirstOrDefault()?.DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Исключение при несуществующем идентификаторе
    /// </summary>
    [Fact]
    public async Task DeleteActShouldThrowNotFoundException()
    {
        // Act
        var act = () => actService.DeleteActAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Act>>();
    }
}
