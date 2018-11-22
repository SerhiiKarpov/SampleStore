namespace SampleStore.Services.Email.SendGrid.Extensions
{
    using System;

    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.DependencyInjection;

    using SampleStore.Common;

    /// <summary>
    /// Class encapsulating service collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        #region Methods

        /// <summary>
        /// Adds the send grid email sender.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="buildOptions">The build options.</param>
        public static void AddSendGridEmailSender(this IServiceCollection services, Action<SendGridEmailSenderOptions> buildOptions)
        {
            buildOptions.ThrowIfArgumentIsNull(nameof(buildOptions));

            var options = new SendGridEmailSenderOptions();
            buildOptions(options);
            services.AddSingleton<IEmailSender>(new SendGridEmailSender(options));
        }

        #endregion Methods
    }
}