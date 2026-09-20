using ActPurchase.Domain.Entities;
using ActPurchase.Entites.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActPurchase.Context
{
    public class ActPurchaseContext : DbContext
    {
        public ActPurchaseContext(DbContextOptions<ActPurchaseContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }
        public DbSet<Act> Acts { get; set; }
        public DbSet<ActItem> ActItems { get; set; }
        public DbSet<Counterparty> Counterparties { get; set; }
        public DbSet<Director> Directors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ActConfiguration).Assembly);
        }
    }
}
