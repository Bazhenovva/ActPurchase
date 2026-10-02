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
/// Тесты для <see cref="ActItemService"/>
/// </summary>
public class ActItemServiceTests : ActPurchaseContextInMemory
{
    private readonly IActItemService actItemService;

    /// <summary>
    /// Инициализирует тестовый класс
    /// </summary>
    public ActItemServiceTests()
    {
        var repository = new ActItemRepository(WriterContext, Context);

        var profile = new ServiceProfile();
        var mapper = new MapperConfiguration(x => x.AddProfile(profile), NullLoggerFactory.Instance).CreateMapper();

        actItemService = new ActItemService(repository, UnitOfWork, mapper);
    }

    /// <summary>
    /// Пустой список позиций
    /// </summary>
    [Fact]
    public async Task GetAllActItemsShouldReturnEmpty()
    {
        // Act
        var items = await actItemService.GetAllActItemsAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// Позиции только указанного акта
    /// </summary>
    [Fact]
    public async Task GetAllActItemsShouldReturnValuesForAct()
    {
        // Arrange
        var actId = Guid.NewGuid();
        var otherActId = Guid.NewGuid();

        var item1 = TestEntityProvider.Shared.Create<ActItem>(x => x.ActId = actId);
        var item2 = TestEntityProvider.Shared.Create<ActItem>(x => x.ActId = otherActId);
        var item3 = TestEntityProvider.Shared.Create<ActItem>(x => x.ActId = actId);
        Context.AddRange(item1, item2, item3);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var items = await actItemService.GetAllActItemsAsync(actId, CancellationToken.None);

        // Assert
        items.Should()
            .NotBeNull()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == item1.Id)
            .And.ContainSingle(x => x.Id == item3.Id);
    }

    /// <summary>
    /// Получение позиции по идентификатору
    /// </summary>
    [Fact]
    public async Task GetActItemByIdShouldReturnValue()
    {
        // Arrange
        var entity = TestEntityProvider.Shared.Create<ActItem>();
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await actItemService.GetActItemByIdAsync(entity.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.Name.Should().Be(entity.Name);
        result.Quantity.Should().Be(entity.Quantity);
        result.Price.Should().Be(entity.Price);
    }

    /// <summary>
    /// Исключение при несуществующем идентификаторе
    /// </summary>
    [Fact]
    public async Task GetActItemByIdShouldThrowNotFoundException()
    {
        // Act
        var act = () => actItemService.GetActItemByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<ActItem>>();
    }

    /// <summary>
    /// Успешное создание позиции с подсчётом суммы
    /// </summary>
    [Fact]
    public async Task CreateActItemShouldWork()
    {
        // Arrange
        var createModel = TestEntityProvider.Shared.Create<ActItemCreateModel>(x =>
        {
            x.Quantity = 3m;
            x.Price = 100m;
        });

        // Act
        await actItemService.CreateActItemAsync(createModel, CancellationToken.None);

        // Assert
        Context.Set<ActItem>().Should().BeEquivalentTo([new
        {
            ActId = createModel.ActId,
            Name = createModel.Name,
            Quantity = createModel.Quantity,
            Price = createModel.Price,
            Sum = 300m
        }]);
    }

    /// <summary>
    /// Успешное обновление позиции с пересчётом суммы
    /// </summary>
    [Fact]
    public async Task UpdateActItemShouldWork()
    {
        // Arrange
        var entity = TestEntityProvider.Shared.Create<ActItem>(x =>
        {
            x.Quantity = 1m;
            x.Price = 50m;
            x.Sum = 50m;
        });
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var updateModel = TestEntityProvider.Shared.Create<ActItemModel>(x =>
        {
            x.Id = entity.Id;
            x.Quantity = 4m;
            x.Price = 25m;
        });

        // Act
        await actItemService.UpdateActItemAsync(updateModel, CancellationToken.None);

        // Assert
        Context.Set<ActItem>().Should().BeEquivalentTo([new
        {
            Name = updateModel.Name,
            Quantity = updateModel.Quantity,
            Price = updateModel.Price,
            Sum = 100m
        }]);
    }

    /// <summary>
    /// Исключение при несуществующем идентификаторе
    /// </summary>
    [Fact]
    public async Task UpdateActItemShouldThrowNotFoundException()
    {
        // Arrange
        var updateModel = TestEntityProvider.Shared.Create<ActItemModel>(x => x.Id = Guid.NewGuid());

        // Act
        var act = () => actItemService.UpdateActItemAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<ActItem>>();
    }

    /// <summary>
    /// Успешное удаление позиции
    /// </summary>
    [Fact]
    public async Task DeleteActItemShouldWork()
    {
        // Arrange
        var entity = TestEntityProvider.Shared.Create<ActItem>();
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        await actItemService.DeleteActItemAsync(entity.Id, CancellationToken.None);

        // Assert
        Context.Set<ActItem>().FirstOrDefault()?.DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Исключение при несуществующем идентификаторе
    /// </summary>
    [Fact]
    public async Task DeleteActItemShouldThrowNotFoundException()
    {
        // Act
        var act = () => actItemService.DeleteActItemAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<ActItem>>();
    }
}
