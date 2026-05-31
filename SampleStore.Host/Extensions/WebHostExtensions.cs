using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using SampleStore.Data.Seed;

namespace SampleStore.Host.Extensions;

public static class WebHostExtensions
{
    private const string SeedingErrorMessage = "An error occurred seeding the DB.";

    public static async Task EnsureSeeded(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var seeder = services.GetRequiredService<IDatabaseSeeder>();
            await seeder.EnsureSeeded();
        }
        catch (Exception x)
        {
            var logger = services.GetRequiredService<ILogger<IDatabaseSeeder>>();
            logger.LogError(x, SeedingErrorMessage);
        }
    }
}
