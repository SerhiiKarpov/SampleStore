using System;

using Microsoft.Extensions.Configuration;

using SampleStore.Data.Seed;
using SampleStore.Data.Seed.Commands;
using SampleStore.Data.Seed.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddDatabaseSeeder(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services
            .AddOptions<SuperAdminOptions>()
            .Configure<IConfiguration>(
                (options, configuration) =>
                     configuration
                        .GetSection(SuperAdminOptions.Key)
                        .Bind(options))
            .ValidateDataAnnotations();

        services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();
        services.AddTransient<IDatabaseSeederCommand, CreateRolesCommand>();
        services.AddTransient<IDatabaseSeederCommand, CreateSuperAdminCommand>();
    }
}