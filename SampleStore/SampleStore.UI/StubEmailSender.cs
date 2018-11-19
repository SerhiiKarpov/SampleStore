namespace SampleStore.UI.Services
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity.UI.Services;

    /// <summary>
    /// Class encapsulating stub email sender.
    /// </summary>
    /// <seealso cref="IEmailSender" />
    public class StubEmailSender : IEmailSender
    {
        #region Methods

        /// <summary>
        /// Sends the email asynchronous.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="htmlMessage">The HTML message.</param>
        /// <returns>
        /// The email asynchronous.
        /// </returns>
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }

        #endregion Methods
    }
}