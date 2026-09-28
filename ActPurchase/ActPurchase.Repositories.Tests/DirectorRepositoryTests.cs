using FluentAssertions;
using Xunit;
using ActPurchase.Domain.Entities;
using ActPurchase.Repositories.Contracts;
using ActPurchase.Context.Tests;
using Ahatornn.TestGenerator;

namespace ActPurchase.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="DirectorRepository"/>
/// </summary>
public class DirectorRepositoryTests : ActPurchaseContextInMemory
{
    private readonly IDirectorRepository repository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="DirectorRepositoryTests"/>
    /// </summary>
    public DirectorRepositoryTests()
    {
        repository = new DirectorRepository(WriterContext, Context);
    }

    /// <summary>
    /// возвращает пустую коллекцию, если БД пустая
    /// </summary>
    [Fact]
    public async Task GetAllDirectorsShouldReturnEmpty()
    {
        // Act
        var result = await repository.GetAllDirectorsAsync(CancellationToken.None);

        // Assert
        result.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// возвращает неудалённых директоров, отсортированных по фамилии
    /// </summary>
    [Fact]
    public async Task GetAllDirectorsShouldReturnValue()
    {
        // Arrange
        var item1 = TestEntityProvider.Shared.Create<Director>(x => x.LastName = "Иванов");
        var item2 = TestEntityProvider.Shared.Create<Director>(x =>
        {
            x.LastName = "Петров";
            x.DeletedAt = DateTimeOffset.Now;
        });
        var item3 = TestEntityProvider.Shared.Create<Director>(x => x.LastName = "Сидоров");

        Context.AddRange(item1, item2, item3);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetAllDirectorsAsync(CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.HaveCount(2)
            .And.BeInAscendingOrder(x => x.LastName)
            .And.ContainSingle(x => x.Id == item1.Id)
            .And.ContainSingle(x => x.Id == item3.Id);
    }

    /// <summary>
    /// возвращает директора по идентификатору
    /// </summary>
    [Fact]
    public async Task GetDirectorByIdShouldReturnValue()
    {
        // Arrange
        var item = TestEntityProvider.Shared.Create<Director>();
        Context.Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetDirectorByIdAsync(item.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(item.Id);
    }

    /// <summary>
    /// возвращает null, если директор не найден
    /// </summary>
    [Fact]
    public async Task GetDirectorByIdShouldReturnNull()
    {
        // Act
        var result = await repository.GetDirectorByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// возвращает null, если директор удалён
    /// </summary>
    [Fact]
    public async Task GetDirectorByIdShouldReturnNullForDeleted()
    {
        // Arrange
        var item = TestEntityProvider.Shared.Create<Director>(x => x.DeletedAt = DateTimeOffset.Now);
        Context.Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetDirectorByIdAsync(item.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// возвращает директоров указанной компании
    /// </summary>
    [Fact]
    public async Task GetDirectorsByCompanyNameShouldReturnValue()
    {
        // Arrange
        var director1 = TestEntityProvider.Shared.Create<Director>(x =>
        {
            x.LastName = "Иванов";
            x.CompanyName = "Компания1";
        });
        var director2 = TestEntityProvider.Shared.Create<Director>(x =>
        {
            x.LastName = "Петров";
            x.CompanyName = "Компания1";
        });
        var director3 = TestEntityProvider.Shared.Create<Director>(x =>
        {
            x.LastName = "Сидоров";
            x.CompanyName = "Компания2";
        });

        Context.AddRange(director1, director2, director3);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetDirectorsByCompanyNameAsync("Компания1", CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.HaveCount(2)
            .And.BeInAscendingOrder(x => x.LastName)
            .And.OnlyContain(x => x.CompanyName == "Компания1");
    }

    /// <summary>
    /// возвращает пустую коллекцию, если директоров компании нет
    /// </summary>
    [Fact]
    public async Task GetDirectorsByCompanyNameShouldReturnEmpty()
    {
        // Act
        var result = await repository.GetDirectorsByCompanyNameAsync("Компания999", CancellationToken.None);

        // Assert
        result.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    ///  возвращает директора по фамилии и имени
    /// </summary>
    [Fact]
    public async Task GetDirectorByFullNameShouldReturnValue()
    {
        // Arrange
        var item = TestEntityProvider.Shared.Create<Director>(x =>
        {
            x.LastName = "Иванов";
            x.FirstName = "Иван";
        });

        Context.Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetDirectorByFullNameAsync("Иванов", "Иван", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(item.Id);
    }

    /// <summary>
    ///  возвращает null, если директор не найден
    /// </summary>
    [Fact]
    public async Task GetDirectorByFullNameShouldReturnNull()
    {
        // Act
        var result = await repository.GetDirectorByFullNameAsync("Неизвестный", "Неизвестный", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// возвращает null, если директор удалён
    /// </summary>
    [Fact]
    public async Task GetDirectorByFullNameShouldReturnNullForDeleted()
    {
        // Arrange
        var item = TestEntityProvider.Shared.Create<Director>(x =>
        {
            x.LastName = "Иванов";
            x.FirstName = "Иван";
            x.DeletedAt = DateTimeOffset.Now;
        });

        Context.Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetDirectorByFullNameAsync("Иванов", "Иван", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
