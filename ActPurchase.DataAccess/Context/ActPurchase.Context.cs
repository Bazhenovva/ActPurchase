using Microsoft.EntityFrameworkCore;

namespace ActPurchase.DataAccess.Context;

public class ActPurchaseContext : DbContext
{
    public ActPurchaseContext(DbContextOptions<ActPurchaseContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}
