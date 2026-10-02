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
/// Тесты для <see cref="DirectorService"/>
/// </summary>
public class DirectorServiceTests : ActPurchaseContextInMemory
{
    private readonly IDirectorService directorService;

    /// <summary>
    /// Инициализирует тестовый класс
    /// </summary>
    public DirectorServiceTests()
    {
        var repository = new DirectorRepository(WriterContext, Context);

        var profile = new ServiceProfile();
        var mapper = new MapperConfiguration(x => x.AddProfile(profile), NullLoggerFactory.Instance).CreateMapper();

        directorService = new DirectorService(repository, UnitOfWork, mapper);
    }

    /// <summary>
    /// Пустой список директоров
    /// </summary>
    [Fact]
    public async Task GetAllDirectorsShouldReturnEmpty()
    {
        // Act
        var items = await directorService.GetAllDirectorsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// Список директоров без удалённых
    /// </summary>
    [Fact]
    public async Task GetAllDirectorsShouldReturnValues()
    {
        // Arrange
        var item1 = TestEntityProvider.Shared.Create<Director>();
        var item2 = TestEntityProvider.Shared.Create<Director>(x => x.DeletedAt = DateTimeOffset.Now);
        var item3 = TestEntityProvider.Shared.Create<Director>();
        Context.AddRange(item1, item2, item3);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var items = await directorService.GetAllDirectorsAsync(CancellationToken.None);

        // Assert
        items.Should()
            .NotBeNull()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == item1.Id)
            .And.ContainSingle(x => x.Id == item3.Id);
    }

    /// <summary>
    /// Получение директора по идентификатору
    /// </summary>
    [Fact]
    public async Task GetDirectorByIdShouldReturnValue()
    {
        // Arrange
        var entity = TestEntityProvider.Shared.Create<Director>();
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await directorService.GetDirectorByIdAsync(entity.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.LastName.Should().Be(entity.LastName);
        result.FirstName.Should().Be(entity.FirstName);
        result.CompanyName.Should().Be(entity.CompanyName);
    }

    /// <summary>
    /// Исключение при несуществующем идентификаторе
    /// </summary>
    [Fact]
    public async Task GetDirectorByIdShouldThrowNotFoundException()
    {
        // Act
        var act = () => directorService.GetDirectorByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Director>>();
    }

    /// <summary>
    /// Успешное создание директора
    /// </summary>
    [Fact]
    public async Task CreateDirectorShouldWork()
    {
        // Arrange
        var createModel = TestEntityProvider.Shared.Create<DirectorCreateModel>();

        // Act
        await directorService.CreateDirectorAsync(createModel, CancellationToken.None);

        // Assert
        Context.Set<Director>().Should().BeEquivalentTo([new
        {
            LastName = createModel.LastName,
            FirstName = createModel.FirstName,
            MiddleName = createModel.MiddleName,
            CompanyName = createModel.CompanyName
        }]);
    }

    /// <summary>
    /// Исключение при дубликате ФИО
    /// </summary>
    [Fact]
    public async Task CreateDirectorShouldThrowExceptionIfFullNameExists()
    {
        // Arrange
        var existing = TestEntityProvider.Shared.Create<Director>(x =>
        {
            x.LastName = "Иванов";
            x.FirstName = "Иван";
        });
        Context.Add(existing);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var createModel = TestEntityProvider.Shared.Create<DirectorCreateModel>(x =>
        {
            x.LastName = "Иванов";
            x.FirstName = "Иван";
        });

        // Act
        var act = () => directorService.CreateDirectorAsync(createModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ActPurchaseException>();
    }

    /// <summary>
    /// Успешное обновление директора
    /// </summary>
    [Fact]
    public async Task UpdateDirectorShouldWork()
    {
        // Arrange
        var entity = TestEntityProvider.Shared.Create<Director>();
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var updateModel = TestEntityProvider.Shared.Create<DirectorModel>(x =>
        {
            x.Id = entity.Id;
            x.LastName = "Петров";
        });

        // Act
        await directorService.UpdateDirectorAsync(updateModel, CancellationToken.None);

        // Assert
        Context.Set<Director>().Should().BeEquivalentTo([new
        {
            LastName = updateModel.LastName,
            FirstName = updateModel.FirstName,
            MiddleName = updateModel.MiddleName,
            CompanyName = updateModel.CompanyName
        }]);
    }

    /// <summary>
    /// Исключение при несуществующем идентификаторе
    /// </summary>
    [Fact]
    public async Task UpdateDirectorShouldThrowNotFoundException()
    {
        // Arrange
        var updateModel = TestEntityProvider.Shared.Create<DirectorModel>(x => x.Id = Guid.NewGuid());

        // Act
        var act = () => directorService.UpdateDirectorAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Director>>();
    }

    /// <summary>
    /// Исключение при дубликате ФИО у другого директора
    /// </summary>
    [Fact]
    public async Task UpdateDirectorShouldThrowExceptionIfFullNameExists()
    {
        // Arrange
        var entity = TestEntityProvider.Shared.Create<Director>(x =>
        {
            x.LastName = "Иванов";
            x.FirstName = "Иван";
        });
        var other = TestEntityProvider.Shared.Create<Director>(x =>
        {
            x.LastName = "Петров";
            x.FirstName = "Пётр";
        });
        Context.AddRange(entity, other);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var updateModel = TestEntityProvider.Shared.Create<DirectorModel>(x =>
        {
            x.Id = entity.Id;
            x.LastName = "Петров";
            x.FirstName = "Пётр";
        });

        // Act
        var act = () => directorService.UpdateDirectorAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ActPurchaseException>();
    }

    /// <summary>
    /// Успешное удаление директора
    /// </summary>
    [Fact]
    public async Task DeleteDirectorShouldWork()
    {
        // Arrange
        var entity = TestEntityProvider.Shared.Create<Director>();
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        await directorService.DeleteDirectorAsync(entity.Id, CancellationToken.None);

        // Assert
        Context.Set<Director>().FirstOrDefault()?.DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Исключение при несуществующем идентификаторе
    /// </summary>
    [Fact]
    public async Task DeleteDirectorShouldThrowNotFoundException()
    {
        // Act
        var act = () => directorService.DeleteDirectorAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Director>>();
    }
}
