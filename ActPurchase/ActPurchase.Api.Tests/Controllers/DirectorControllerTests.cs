using System.Net;
using System.Net.Http.Json;
using ActPurchase.Api.Models;
using ActPurchase.Api.Tests.Infrastructure;
using ActPurchase.Domain.Entites;
using FluentAssertions;

namespace ActPurchase.Api.Tests.Controllers;

/// <summary>
/// Интеграционные тесты DirectorController
/// </summary>
[Collection(nameof(ActPurchaseApiTestCollection))]
public class DirectorControllerTests : IntegrationTestBase
{
    public DirectorControllerTests(ActPurchaseApiFixture fixture) : base(fixture) { }

    /// <summary>
    /// GET /Director возвращает всех директоров
    /// </summary>
    [Fact]
    public async Task Get_ReturnsAllDirectors()
    {
        var expected = await SeedAsync();
        await SeedAsync();

        var client = CreateClient();
        var response = await client.GetAsync("/Director");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await response.Content.ReadFromJsonAsync<List<DirectorApiModel>>();
        list.Should().NotBeNull();
        list!.Should().Contain(x => x.Id == expected.Id);
    }

    /// <summary>
    /// GET /Director/{id} возвращает директора
    /// </summary>
    [Fact]
    public async Task GetById_ReturnsDirector()
    {
        var seeded = await SeedAsync();

        var client = CreateClient();
        var response = await client.GetAsync($"/Director/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var entity = await response.Content.ReadFromJsonAsync<DirectorApiModel>();
        entity.Should().NotBeNull();
        entity!.Id.Should().Be(seeded.Id);
        entity.LastName.Should().Be(seeded.LastName);
    }

    /// <summary>
    /// GET /Director/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task GetById_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.GetAsync($"/Director/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// POST /Director создаёт директора
    /// </summary>
    [Fact]
    public async Task Create_CreatesDirector()
    {
        var request = new DirectorCreateApiModel
        {
            LastName = $"Ф-{Guid.NewGuid():N}"[..15],
            FirstName = "Имя",
            MiddleName = "Отчество",
            CompanyName = "ООО Тест",
        };

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/Director", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var created = await response.Content.ReadFromJsonAsync<DirectorApiModel>();
        created.Should().NotBeNull();
        created!.Id.Should().NotBe(Guid.Empty);
        created.LastName.Should().Be(request.LastName);
    }

    /// <summary>
    /// POST /Director с дубликатом ФИО возвращает 406
    /// </summary>
    [Fact]
    public async Task Create_WhenFullNameExists_Returns406()
    {
        var seeded = await SeedAsync();
        var request = new DirectorCreateApiModel
        {
            LastName = seeded.LastName,
            FirstName = seeded.FirstName,
            MiddleName = seeded.MiddleName,
            CompanyName = "Другая",
        };

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/Director", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
    }

    /// <summary>
    /// PUT /Director/{id} обновляет директора
    /// </summary>
    [Fact]
    public async Task Update_UpdatesDirector()
    {
        var seeded = await SeedAsync();
        var request = new DirectorCreateApiModel
        {
            LastName = "Обновлённая",
            FirstName = seeded.FirstName,
            MiddleName = seeded.MiddleName,
            CompanyName = seeded.CompanyName,
        };

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/Director/{seeded.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var inDb = Context.Set<Director>().FirstOrDefault(x => x.Id == seeded.Id);
        inDb!.LastName.Should().Be("Обновлённая");
    }

    /// <summary>
    /// PUT /Director/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task Update_WhenNotFound_Returns404()
    {
        var request = new DirectorCreateApiModel
        {
            LastName = "Тест",
            FirstName = "Тест",
            CompanyName = "Тест",
        };

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/Director/{Guid.NewGuid()}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// DELETE /Director/{id} мягко удаляет директора
    /// </summary>
    [Fact]
    public async Task Delete_DeletesDirector()
    {
        var seeded = await SeedAsync();

        var client = CreateClient();
        var response = await client.DeleteAsync($"/Director/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var inDb = Context.Set<Director>().FirstOrDefault(x => x.Id == seeded.Id);
        inDb!.DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// DELETE /Director/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task Delete_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.DeleteAsync($"/Director/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Создаёт директора
    /// </summary>
    private async Task<Director> SeedAsync()
    {
        var entity = new Director
        {
            Id = Guid.NewGuid(),
            LastName = $"Ф-{Guid.NewGuid():N}"[..15],
            FirstName = "Имя",
            MiddleName = "Отчество",
            CompanyName = "ООО Тест",
        };
        Context.Set<Director>().Add(entity);
        await UnitOfWork.SaveChangesAsync();
        return entity;
    }
}
