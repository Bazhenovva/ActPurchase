using System.Net;
using System.Net.Http.Json;
using ActPurchase.Api.Models;
using ActPurchase.Api.Tests.Infrastructure;
using ActPurchase.Domain.Entites;
using ActPurchase.Domain.Enum;
using FluentAssertions;

namespace ActPurchase.Api.Tests.Controllers;

/// <summary>
/// Интеграционные тесты ActController
/// </summary>
[Collection(nameof(ActPurchaseApiTestCollection))]
public class ActControllerTests : IntegrationTestBase
{
    public ActControllerTests(ActPurchaseApiFixture fixture) : base(fixture) { }

    /// <summary>
    /// GET /Act возвращает все акты
    /// </summary>
    [Fact]
    public async Task Get_ReturnsAllActs()
    {
        var expected = await SeedActAsync();
        await SeedActAsync();

        var client = CreateClient();
        var response = await client.GetAsync("/Act");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var acts = await response.Content.ReadFromJsonAsync<List<ActApiModel>>();
        acts.Should().NotBeNull();
        acts!.Should().Contain(x => x.Id == expected.Id && x.Number == expected.Number);
    }

    /// <summary>
    /// GET /Act/{id} возвращает акт
    /// </summary>
    [Fact]
    public async Task GetById_ReturnsAct()
    {
        var seeded = await SeedActAsync();

        var client = CreateClient();
        var response = await client.GetAsync($"/Act/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var act = await response.Content.ReadFromJsonAsync<ActApiModel>();
        act.Should().NotBeNull();
        act!.Id.Should().Be(seeded.Id);
        act.Number.Should().Be(seeded.Number);
        act.City.Should().Be(seeded.City);
    }

    /// <summary>
    /// GET /Act/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task GetById_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.GetAsync($"/Act/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// GET /Act/{id} с не-Guid возвращает 404
    /// </summary>
    [Fact]
    public async Task GetById_WhenIdIsNotGuid_Returns404()
    {
        var client = CreateClient();
        var response = await client.GetAsync("/Act/not-a-guid");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// POST /Act создаёт акт
    /// </summary>
    [Fact]
    public async Task Create_CreatesAct()
    {
        var (director, buyer, seller) = await SeedDependenciesAsync();
        var request = new ActCreateApiModel
        {
            Number = $"А-{Guid.NewGuid():N}"[..15],
            City = "Москва",
            Date = DateOnly.FromDateTime(DateTime.Now),
            Type = "Купля-продажа",
            BuyerId = buyer.Id,
            SellerId = seller.Id,
            DirectorId = director.Id,
        };

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/Act", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var created = await response.Content.ReadFromJsonAsync<ActApiModel>();
        created.Should().NotBeNull();
        created!.Id.Should().NotBe(Guid.Empty);
        created.Number.Should().Be(request.Number);
        created.City.Should().Be(request.City);

        var inDb = Context.Set<Act>().FirstOrDefault(x => x.Id == created.Id);
        inDb.Should().NotBeNull();
        inDb!.Number.Should().Be(request.Number);
    }

    /// <summary>
    /// POST /Act с дубликатом номера возвращает 406
    /// </summary>
    [Fact]
    public async Task Create_WhenNumberExists_Returns406()
    {
        var seeded = await SeedActAsync();
        var request = new ActCreateApiModel
        {
            Number = seeded.Number,
            City = "Москва",
            Date = DateOnly.FromDateTime(DateTime.Now),
            Type = "Купля-продажа",
            BuyerId = seeded.BuyerId,
            SellerId = seeded.SellerId,
            DirectorId = seeded.DirectorId,
        };

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/Act", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
    }

    /// <summary>
    /// PUT /Act/{id} обновляет акт
    /// </summary>
    [Fact]
    public async Task Update_UpdatesAct()
    {
        var seeded = await SeedActAsync();
        var request = new ActCreateApiModel
        {
            Number = $"О-{Guid.NewGuid():N}"[..15],
            City = "Новосибирск",
            Date = DateOnly.FromDateTime(DateTime.Now),
            Type = "Поставка",
            BuyerId = seeded.BuyerId,
            SellerId = seeded.SellerId,
            DirectorId = seeded.DirectorId,
        };

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/Act/{seeded.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var inDb = Context.Set<Act>().FirstOrDefault(x => x.Id == seeded.Id);
        inDb.Should().NotBeNull();
        inDb!.Number.Should().Be(request.Number);
        inDb.City.Should().Be(request.City);
    }

    /// <summary>
    /// PUT /Act/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task Update_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var request = new ActCreateApiModel
        {
            Number = $"А-{Guid.NewGuid():N}"[..15],
            City = "Москва",
            Date = DateOnly.FromDateTime(DateTime.Now),
            Type = "Купля-продажа",
            BuyerId = Guid.NewGuid(),
            SellerId = Guid.NewGuid(),
            DirectorId = Guid.NewGuid(),
        };

        var response = await client.PutAsJsonAsync($"/Act/{Guid.NewGuid()}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// PUT /Act/{id} с чужим номером возвращает 406
    /// </summary>
    [Fact]
    public async Task Update_WhenNumberTakenByOther_Returns406()
    {
        var first = await SeedActAsync();
        var second = await SeedActAsync();
        var request = new ActCreateApiModel
        {
            Number = first.Number,
            City = "Москва",
            Date = DateOnly.FromDateTime(DateTime.Now),
            Type = "Купля-продажа",
            BuyerId = second.BuyerId,
            SellerId = second.SellerId,
            DirectorId = second.DirectorId,
        };

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/Act/{second.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
    }

    /// <summary>
    /// DELETE /Act/{id} мягко удаляет акт
    /// </summary>
    [Fact]
    public async Task Delete_DeletesAct()
    {
        var seeded = await SeedActAsync();

        var client = CreateClient();
        var response = await client.DeleteAsync($"/Act/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var inDb = Context.Set<Act>().FirstOrDefault(x => x.Id == seeded.Id);
        inDb.Should().NotBeNull();
        inDb!.DeletedAt.Should().NotBeNull();

        var getResponse = await client.GetAsync($"/Act/{seeded.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// DELETE /Act/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task Delete_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.DeleteAsync($"/Act/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Создаёт директора и двух контрагентов
    /// </summary>
    private async Task<(Director director, Counterparty buyer, Counterparty seller)> SeedDependenciesAsync()
    {
        var director = new Director
        {
            Id = Guid.NewGuid(),
            LastName = $"Ф-{Guid.NewGuid():N}"[..15],
            FirstName = "Имя",
            CompanyName = "ООО Тест",
        };
        var buyer = new Counterparty
        {
            Id = Guid.NewGuid(),
            Type = CounterpartyType.Buyer,
            CompanyName = $"Покупатель-{Guid.NewGuid():N}",
            Inn = Guid.NewGuid().ToString("N")[..10],
            Kpp = "123456789",
            Ogrn = "1234567890123",
            CheckingAccount = "40702810123456789012",
            CorrespondentAccount = "30101810400000000225",
            Bik = "044525225",
            BankName = "Сбербанк",
            LegalAddress = "Москва",
        };
        var seller = new Counterparty
        {
            Id = Guid.NewGuid(),
            Type = CounterpartyType.Seller,
            CompanyName = $"Продавец-{Guid.NewGuid():N}",
            Inn = Guid.NewGuid().ToString("N")[..10],
            Kpp = "987654321",
            Ogrn = "3210987654321",
            CheckingAccount = "40702810987654321098",
            CorrespondentAccount = "30101810400000000225",
            Bik = "044525225",
            BankName = "ВТБ",
            LegalAddress = "СПб",
        };
        Context.Set<Director>().Add(director);
        Context.Set<Counterparty>().AddRange(buyer, seller);
        await UnitOfWork.SaveChangesAsync();
        return (director, buyer, seller);
    }

    /// <summary>
    /// Создаёт акт с зависимостями
    /// </summary>
    private async Task<Act> SeedActAsync()
    {
        var (director, buyer, seller) = await SeedDependenciesAsync();
        var entity = new Act
        {
            Id = Guid.NewGuid(),
            Number = $"А-{Guid.NewGuid():N}"[..15],
            City = "Москва",
            Date = DateOnly.FromDateTime(DateTime.Now),
            Type = "Купля-продажа",
            BuyerId = buyer.Id,
            SellerId = seller.Id,
            DirectorId = director.Id,
        };
        Context.Set<Act>().Add(entity);
        await UnitOfWork.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// GET /Act/{id}/excel возвращает Excel-файл
    /// </summary>
    [Fact]
    public async Task GetActExcel_ReturnsFile()
    {
        var seeded = await SeedActAsync();

        var client = CreateClient();
        var response = await client.GetAsync($"/Act/{seeded.Id}/excel");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType!.MediaType
            .Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        var bytes = await response.Content.ReadAsByteArrayAsync();
        bytes.Should().NotBeEmpty();
    }
}
