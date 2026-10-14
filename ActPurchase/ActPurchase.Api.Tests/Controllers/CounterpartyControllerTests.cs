using System.Net;
using System.Net.Http.Json;
using ActPurchase.Api.Models;
using ActPurchase.Api.Tests.Infrastructure;
using ActPurchase.Domain.Entites;
using ActPurchase.Domain.Enum;
using FluentAssertions;

namespace ActPurchase.Api.Tests.Controllers;

/// <summary>
/// Интеграционные тесты CounterpartyController
/// </summary>
[Collection(nameof(ActPurchaseApiTestCollection))]
public class CounterpartyControllerTests : IntegrationTestBase
{
    public CounterpartyControllerTests(ActPurchaseApiFixture fixture) : base(fixture) { }

    /// <summary>
    /// GET /Counterparty возвращает всех контрагентов
    /// </summary>
    [Fact]
    public async Task Get_ReturnsAllCounterparties()
    {
        var expected = await SeedAsync();
        await SeedAsync();

        var client = CreateClient();
        var response = await client.GetAsync("/Counterparty");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await response.Content.ReadFromJsonAsync<List<CounterpartyApiModel>>();
        list.Should().NotBeNull();
        list!.Should().Contain(x => x.Id == expected.Id);
    }

    /// <summary>
    /// GET /Counterparty/{id} возвращает контрагента
    /// </summary>
    [Fact]
    public async Task GetById_ReturnsCounterparty()
    {
        var seeded = await SeedAsync();

        var client = CreateClient();
        var response = await client.GetAsync($"/Counterparty/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var entity = await response.Content.ReadFromJsonAsync<CounterpartyApiModel>();
        entity.Should().NotBeNull();
        entity!.Id.Should().Be(seeded.Id);
        entity.CompanyName.Should().Be(seeded.CompanyName);
    }

    /// <summary>
    /// GET /Counterparty/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task GetById_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.GetAsync($"/Counterparty/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// POST /Counterparty создаёт контрагента
    /// </summary>
    [Fact]
    public async Task Create_CreatesCounterparty()
    {
        var request = new CounterpartyCreateApiModel
        {
            Type = CounterpartyType.Buyer,
            CompanyName = $"Компания-{Guid.NewGuid():N}",
            Inn = Guid.NewGuid().ToString("N")[..10],
            Kpp = "123456789",
            Ogrn = "1234567890123",
            CheckingAccount = "40702810123456789012",
            CorrespondentAccount = "30101810400000000225",
            Bik = "044525225",
            BankName = "Сбербанк",
            LegalAddress = "Москва",
        };

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/Counterparty", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var created = await response.Content.ReadFromJsonAsync<CounterpartyApiModel>();
        created.Should().NotBeNull();
        created!.Id.Should().NotBe(Guid.Empty);
        created.CompanyName.Should().Be(request.CompanyName);
    }

    /// <summary>
    /// POST /Counterparty с дубликатом ИНН возвращает 406
    /// </summary>
    [Fact]
    public async Task Create_WhenInnExists_Returns406()
    {
        var seeded = await SeedAsync();
        var request = new CounterpartyCreateApiModel
        {
            Type = CounterpartyType.Buyer,
            CompanyName = "Другая",
            Inn = seeded.Inn,
            Kpp = "123456789",
            Ogrn = "1234567890123",
            CheckingAccount = "40702810123456789012",
            CorrespondentAccount = "30101810400000000225",
            Bik = "044525225",
            BankName = "Сбербанк",
            LegalAddress = "Москва",
        };

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/Counterparty", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
    }

    /// <summary>
    /// PUT /Counterparty/{id} обновляет контрагента
    /// </summary>
    [Fact]
    public async Task Update_UpdatesCounterparty()
    {
        var seeded = await SeedAsync();
        var request = new CounterpartyCreateApiModel
        {
            Type = seeded.Type,
            CompanyName = "Обновлённая",
            Inn = seeded.Inn,
            Kpp = seeded.Kpp,
            Ogrn = seeded.Ogrn,
            CheckingAccount = seeded.CheckingAccount,
            CorrespondentAccount = seeded.CorrespondentAccount,
            Bik = seeded.Bik,
            BankName = seeded.BankName,
            LegalAddress = seeded.LegalAddress,
        };

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/Counterparty/{seeded.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var inDb = Context.Set<Counterparty>().FirstOrDefault(x => x.Id == seeded.Id);
        inDb!.CompanyName.Should().Be("Обновлённая");
    }

    /// <summary>
    /// PUT /Counterparty/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task Update_WhenNotFound_Returns404()
    {
        var request = new CounterpartyCreateApiModel
        {
            Type = CounterpartyType.Buyer,
            CompanyName = "Тест",
            Inn = Guid.NewGuid().ToString("N")[..10],
            Kpp = "123456789",
            Ogrn = "1234567890123",
            CheckingAccount = "40702810123456789012",
            CorrespondentAccount = "30101810400000000225",
            Bik = "044525225",
            BankName = "Сбербанк",
            LegalAddress = "Москва",
        };

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/Counterparty/{Guid.NewGuid()}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// DELETE /Counterparty/{id} мягко удаляет контрагента
    /// </summary>
    [Fact]
    public async Task Delete_DeletesCounterparty()
    {
        var seeded = await SeedAsync();

        var client = CreateClient();
        var response = await client.DeleteAsync($"/Counterparty/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var inDb = Context.Set<Counterparty>().FirstOrDefault(x => x.Id == seeded.Id);
        inDb!.DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// DELETE /Counterparty/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task Delete_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.DeleteAsync($"/Counterparty/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Создаёт контрагента
    /// </summary>
    private async Task<Counterparty> SeedAsync()
    {
        var entity = new Counterparty
        {
            Id = Guid.NewGuid(),
            Type = CounterpartyType.Buyer,
            CompanyName = $"Компания-{Guid.NewGuid():N}",
            Inn = Guid.NewGuid().ToString("N")[..10],
            Kpp = "123456789",
            Ogrn = "1234567890123",
            CheckingAccount = "40702810123456789012",
            CorrespondentAccount = "30101810400000000225",
            Bik = "044525225",
            BankName = "Сбербанк",
            LegalAddress = "Москва",
        };
        Context.Set<Counterparty>().Add(entity);
        await UnitOfWork.SaveChangesAsync();
        return entity;
    }
}
