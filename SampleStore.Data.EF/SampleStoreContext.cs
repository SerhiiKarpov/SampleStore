using Microsoft.EntityFrameworkCore;

namespace SampleStore.Data.EF;

internal class SampleStoreContext : DbContext
{
    public SampleStoreContext(DbContextOptions<SampleStoreContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(SampleStoreContext).Assembly);
    }
}