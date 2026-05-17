using Microsoft.Extensions.DependencyInjection;

using SampleStore.Common.Extensions;
using SampleStore.Data.Seed.Commands;

namespace SampleStore.Data.Seed.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDatabaseSeeder(this IServiceCollection services)
    {
        services.ThrowIfArgumentIsNull(nameof(services));

        services.AddScoped<DatabaseSeeder>();
        services.AddTransient<ICreateSuperAdminCommandFactory, SeederCommandFactory>();
        services.AddTransient<ICreateRolesCommandFactory, SeederCommandFactory>();
        services.AddTransient<IAddUserToRolesCommandFactory, SeederCommandFactory>();
    }
}