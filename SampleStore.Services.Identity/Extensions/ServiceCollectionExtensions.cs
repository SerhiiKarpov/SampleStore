using System;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCustomizedIdentity(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddIdentity<User, Role>()
            .AddDefaultTokenProviders();

        services.AddScoped<IUserStore<User>, UserStore>();
        services.AddScoped<IRoleStore<Role>, RoleStore>();
    }
}