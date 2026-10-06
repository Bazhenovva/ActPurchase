using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ActPurchase.Context.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Counterparties",
                columns: new[] { "Id", "BankName", "Bik", "CheckingAccount", "CompanyName", "CorrespondentAccount", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Inn", "Kpp", "LegalAddress", "Ogrn", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Сбербанк", "044525225", "40817810099910004312", "ООО Озон", "30101810400000000225", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", null, null, "1234567890", "123456789", "г. Москва, ул. Ленина, д. 1", "1234567890123", "Buyer", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "ВТБ", "044525225", "40817810799910005432", "ООО Вб", "30101810400000000225", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", null, null, "0987654321", "987654321", "г. Санкт-Петербург, ул. Пушкина, д. 10", "3210987654321", "Seller", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System" }
                });

            migrationBuilder.InsertData(
                table: "Directors",
                columns: new[] { "Id", "CompanyName", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "FirstName", "LastName", "MiddleName", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444444"), "ООО Озон", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", null, null, "Иван", "Иванов", "Иванович", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "ООО Вб", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", null, null, "Пётр", "Петров", "Петрович", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System" }
                });

            migrationBuilder.InsertData(
                table: "Acts",
                columns: new[] { "Id", "BuyerId", "City", "CreatedAt", "CreatedBy", "Date", "DeletedAt", "DeletedBy", "DirectorId", "Number", "SellerId", "TotalQuantity", "TotalSum", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("11111111-1111-1111-1111-111111111111"), "Москва", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", new DateOnly(2026, 1, 15), null, null, new Guid("44444444-4444-4444-4444-444444444444"), "001", new Guid("22222222-2222-2222-2222-222222222222"), 6, 16000m, "Купля-продажа", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new Guid("22222222-2222-2222-2222-222222222222"), "Санкт-Петербург", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", new DateOnly(2026, 2, 20), null, null, new Guid("55555555-5555-5555-5555-555555555555"), "002", new Guid("11111111-1111-1111-1111-111111111111"), 0, 0m, "Поставка", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System" }
                });

            migrationBuilder.InsertData(
                table: "ActItems",
                columns: new[] { "Id", "ActId", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Name", "Price", "Quantity", "Sum", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("33333333-3333-3333-3333-333333333333"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", null, null, "Стол письменный", 5000m, 2m, 10000m, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("33333333-3333-3333-3333-333333333333"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", null, null, "Стул офисный", 1500m, 4m, 6000m, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActItems",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "ActItems",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "Acts",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "Acts",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Counterparties",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Counterparties",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));
        }
    }
}
