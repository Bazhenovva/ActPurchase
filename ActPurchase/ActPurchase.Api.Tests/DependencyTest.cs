using System.Reflection;
using ActPurchase.Api.Controllers;
using ActPurchase.Api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ActPurchase.Api.Tests;

/// <summary>
/// Проверка, что все контроллеры резолвятся из DI
/// </summary>
public class DependencyTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;

    /// <summary>
    /// Инициализирует тестовый класс с настроенной фабрикой
    /// </summary>
    public DependencyTest(WebApplicationFactory<Program> factory)
    {
        this.factory = factory.WithWebHostBuilder(builder => builder.ConfigureTestAppConfiguration());
    }

    /// <summary>
    /// Контроллер должен резолвиться из DI
    /// </summary>
    [Theory]
    [MemberData(nameof(Controllers))]
    public void ControllerShouldBeResolved(Type controller)
    {
        using var scope = factory.Services.CreateScope();
        var instance = scope.ServiceProvider.GetRequiredService(controller);
        Assert.NotNull(instance);
    }

    /// <summary>
    /// Все контроллеры приложения
    /// </summary>
    public static TheoryData<Type> Controllers => GetControllers<ActController>();

    private static TheoryData<Type> GetControllers<TController>() =>
        new(Assembly.GetAssembly(typeof(TController))
                ?.DefinedTypes
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract)
            ?? []);
}
