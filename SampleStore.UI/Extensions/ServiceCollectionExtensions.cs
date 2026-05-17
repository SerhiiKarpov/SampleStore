using Microsoft.Extensions.DependencyInjection;

using SampleStore.UI.Configuration;

namespace SampleStore.UI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureUI(this IServiceCollection services)
    {
        services.ConfigureOptions<RclStaticFileOptions>();
    }
}