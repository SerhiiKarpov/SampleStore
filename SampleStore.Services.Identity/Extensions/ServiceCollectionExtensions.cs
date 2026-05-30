using System;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomizedIdentity(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddIdentity<User, Role>()
            .AddDefaultTokenProviders();

        return services
            .AddScoped<IUserManager, UserManagerWrapper>()
            .AddScoped<IUserStore<User>, UserStore>()
            .AddScoped<IRoleManager, RoleManagerWrapper>()
            .AddScoped<IRoleStore<Role>, RoleStore>()
            .AddScoped<ISignInManager, SignInManagerWrapper>();
    }
}