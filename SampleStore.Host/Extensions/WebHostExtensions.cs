using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using SampleStore.Data.Entities.Identity;
using SampleStore.Data.Seed;

namespace SampleStore.Host.Extensions;

public static class WebHostExtensions
{
    private const string SeederPasswordKey = "SeederPassword";

    private const string SeedingErrorMessage = "An error occurred creating the DB.";

    private const string SuperAdminPrototypeSection = "SuperAdminPrototype";

    public static async Task EnsureSeeded(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var configuration = services.GetRequiredService<IConfiguration>();
            var superAdminPrototype = configuration.GetSection(SuperAdminPrototypeSection).Get<User>();
            var seederPassword = configuration[SeederPasswordKey];

            var seeder = services.GetRequiredService<DatabaseSeeder>();
            var needsSeeding = await seeder.NeedsSeeding();
            if (needsSeeding)
            {
                await seeder.Seed(superAdminPrototype!, seederPassword!);
            }
        }
        catch (Exception x)
        {
            var logger = services.GetRequiredService<ILogger<DatabaseSeeder>>();
            logger.LogError(x, SeedingErrorMessage);
        }
    }
}
