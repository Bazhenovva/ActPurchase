using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ActPurchase.Context
{
    public class ActPurchaseDesignTimeContextFactory : IDesignTimeDbContextFactory<ActPurchaseContext>
    {
        /// <summary>
        /// Creates a new instance of a derived context
        /// </summary>
        /// <remarks>
        /// 1) dotnet tool install --global dotnet-ef
        /// 2) dotnet tool update --global dotnet-ef
        /// 3) dotnet ef migrations add [name] --project DataAccessLayer/FinalExercise.Context/FinalExercise.Context.csproj
        /// 4) dotnet ef database update --project DataAccessLayer/FinalExercise.Context/FinalExercise.Context.csproj
        /// 5) dotnet ef database update [targetMigrationName] --project DataAccessLayer/FinalExercise.Context/FinalExercise.Context.csproj
        /// </remarks>
        public ActPurchaseContext CreateDbContext(string[] args)
        {
            var connectionString = "Host=localhost;Port=4321;Database=postgres;Username=postgres;Password=1234567890";

            var options = new DbContextOptionsBuilder<ActPurchaseContext>()
                .UseNpgsql(connectionString)
                .LogTo(Console.WriteLine)
                .Options;

            return new ActPurchaseContext(options);
        }
    }
}
