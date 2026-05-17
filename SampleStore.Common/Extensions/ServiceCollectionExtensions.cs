using Microsoft.Extensions.DependencyInjection;

using SampleStore.Common.Services;

namespace SampleStore.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTime, DateTimeService>();

        return services;
    }
}