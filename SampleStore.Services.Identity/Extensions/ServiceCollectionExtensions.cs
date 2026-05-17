using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using SampleStore.Common.Extensions;
using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCustomizedIdentity(this IServiceCollection services)
    {
        services.ThrowIfArgumentIsNull(nameof(services));

        services.AddIdentity<User, Role>()
            .AddDefaultTokenProviders();

        services.AddScoped<IUserStore<User>, UserStore>();
        services.AddScoped<IRoleStore<Role>, RoleStore>();
    }
}