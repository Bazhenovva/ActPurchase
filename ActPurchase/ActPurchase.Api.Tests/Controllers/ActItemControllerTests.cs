using System.Net;
using System.Net.Http.Json;
using ActPurchase.Api.Models;
using ActPurchase.Api.Tests.Infrastructure;
using ActPurchase.Domain.Entites;
using ActPurchase.Domain.Enum;
using FluentAssertions;

namespace ActPurchase.Api.Tests.Controllers;

/// <summary>
/// Интеграционные тесты ActItemController
/// </summary>
[Collection(nameof(ActPurchaseApiTestCollection))]
public class ActItemControllerTests : IntegrationTestBase
{
    public ActItemControllerTests(ActPurchaseApiFixture fixture) : base(fixture) { }

    /// <summary>
    /// GET /ActItem возвращает позиции указанного акта
    /// </summary>
    [Fact]
    public async Task Get_ReturnsActItemsForAct()
    {
        var (act, _) = await SeedActWithItemAsync();

        var client = CreateClient();
        var response = await client.GetAsync($"/ActItem?actId={act.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    /// <summary>
    /// GET /ActItem/{id} возвращает позицию
    /// </summary>
    [Fact]
    public async Task GetById_ReturnsActItem()
    {
        var (_, item) = await SeedActWithItemAsync();

        var client = CreateClient();
        var response = await client.GetAsync($"/ActItem/{item.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var entity = await response.Content.ReadFromJsonAsync<ActItemApiModel>();
        entity.Should().NotBeNull();
        entity!.Id.Should().Be(item.Id);
        entity.Name.Should().Be(item.Name);
    }

    /// <summary>
    /// GET /ActItem/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task GetById_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.GetAsync($"/ActItem/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// POST /ActItem создаёт позицию с подсчётом суммы
    /// </summary>
    [Fact]
    public async Task Create_CreatesActItem()
    {
        var (act, _) = await SeedActWithItemAsync();
        var request = new ActItemCreateApiModel
        {
            ActId = act.Id,
            Name = $"Позиция-{Guid.NewGuid():N}",
            Quantity = 3m,
            Price = 100m,
        };

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/ActItem", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var created = await response.Content.ReadFromJsonAsync<ActItemApiModel>();
        created.Should().NotBeNull();
        created!.Sum.Should().Be(300m);
    }

    /// <summary>
    /// PUT /ActItem/{id} пересчитывает сумму
    /// </summary>
    [Fact]
    public async Task Update_UpdatesActItem()
    {
        var (_, item) = await SeedActWithItemAsync();
        var request = new ActItemCreateApiModel
        {
            ActId = item.ActId,
            Name = item.Name,
            Quantity = 4m,
            Price = 25m,
        };

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/ActItem/{item.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var inDb = Context.Set<ActItem>().FirstOrDefault(x => x.Id == item.Id);
        inDb!.Sum.Should().Be(100m);
    }

    /// <summary>
    /// DELETE /ActItem/{id} мягко удаляет позицию
    /// </summary>
    [Fact]
    public async Task Delete_DeletesActItem()
    {
        var (_, item) = await SeedActWithItemAsync();

        var client = CreateClient();
        var response = await client.DeleteAsync($"/ActItem/{item.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var inDb = Context.Set<ActItem>().FirstOrDefault(x => x.Id == item.Id);
        inDb!.DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Создаёт акт с позицией
    /// </summary>
    private async Task<(Act act, ActItem item)> SeedActWithItemAsync()
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
        var act = new Act
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
        var item = new ActItem
        {
            Id = Guid.NewGuid(),
            ActId = act.Id,
            Name = $"Позиция-{Guid.NewGuid():N}",
            Quantity = 2m,
            Price = 50m,
            Sum = 100m,
        };
        Context.Set<Director>().Add(director);
        Context.Set<Counterparty>().AddRange(buyer, seller);
        Context.Set<Act>().Add(act);
        Context.Set<ActItem>().Add(item);
        await UnitOfWork.SaveChangesAsync();
        return (act, item);
    }
}
