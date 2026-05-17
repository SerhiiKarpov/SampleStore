using System;

using Microsoft.AspNetCore.Identity.UI.Services;

using SampleStore.Services.Email.SendGrid;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSendGridEmailSender(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services
            .AddOptions<SendGridEmailSenderOptions>()
            .ValidateDataAnnotations();

        services.AddScoped<IEmailSender, SendGridEmailSender>();

        return services;
    }
}