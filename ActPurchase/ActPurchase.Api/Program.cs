using ActPurchase.Api.Automapper;
using ActPurchase.Api.Implementations;
using ActPurchase.Api.Infrastructure;
using ActPurchase.Common;
using ActPurchase.Context;
using ActPurchase.Dal.Contracts.Repositories;
using ActPurchase.Repositories;
using ActPurchase.Repositories.Contracts;
using ActPurchase.Services.Services;
using ActPurchase.Services.Automapper;
using ActPurchase.Services.Contracts.Interfaces;
using ActPurchase.Services.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddSingleton<IIdentityProvider, IdentityProvider>();

builder.Services.AddScoped<IValidateService, ValidateService>();
builder.Services.RegisterImplementationsOf<IValidator>(
    typeof(ActCreateModelValidator).Assembly,
    ServiceLifetime.Scoped);

builder.Services.AddScoped<IActRepository, ActRepository>();
builder.Services.AddScoped<IActItemRepository, ActItemRepository>();
builder.Services.AddScoped<ICounterpartyRepository, CounterpartyRepository>();
builder.Services.AddScoped<IDirectorRepository, DirectorRepository>();

builder.Services.AddScoped<IActService, ActService>();
builder.Services.AddScoped<IActItemService, ActItemService>();
builder.Services.AddScoped<ICounterpartyService, CounterpartyService>();
builder.Services.AddScoped<IDirectorService, DirectorService>();

builder.Services.AddScoped<IDbWriterContext, DbWriterContext>();

builder.Services.AddAutoMapper(x =>
{
    x.AddProfile<ApiProfile>();
    x.AddProfile<ServiceProfile>();
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ActPurchaseContext>(opts =>
    opts.UseNpgsql(connectionString));
builder.Services.AddScoped<IUnitOfWork>(x => x.GetRequiredService<ActPurchaseContext>());
builder.Services.AddScoped<IReader>(x => x.GetRequiredService<ActPurchaseContext>());
builder.Services.AddScoped<IWriter>(x => x.GetRequiredService<ActPurchaseContext>());

var controllers = builder.Services.AddControllers(opts => opts.Filters.Add<ExceptionFilter>());
if (builder.Environment.IsEnvironment("integration"))
{
    controllers.AddControllersAsServices();
}

builder.Services.AddHealthChecks();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("swagger/v1/swagger.json");
    app.UseSwaggerUI();
}

app.MapHealthChecks("health");
app.MapControllers();

app.Run();

public partial class Program { }
