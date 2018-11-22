namespace SampleStore.Services.Email.SendGrid
{
    using System.Threading.Tasks;

    using global::SendGrid;
    using global::SendGrid.Helpers.Mail;

    using Microsoft.AspNetCore.Identity.UI.Services;

    using SampleStore.Common;

    /// <summary>
    /// Class encapsulating send grid email sender.
    /// </summary>
    /// <seealso cref="IEmailSender" />
    public class SendGridEmailSender : IEmailSender
    {
        #region Fields

        /// <summary>
        /// The options
        /// </summary>
        private readonly SendGridEmailSenderOptions _options;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SendGridEmailSender"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public SendGridEmailSender(SendGridEmailSenderOptions options)
        {
            _options = options.ThrowIfArgumentIsNull(nameof(options));
        }

        #endregion Constructors

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
            var client = new SendGridClient(_options.SendGridKey);
            var message = new SendGridMessage
            {
                From = new EmailAddress(_options.SenderEmail),
                Subject = subject,
                HtmlContent = htmlMessage
            };
            message.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            message.SetClickTracking(false, false);

            return client.SendEmailAsync(message);
        }

        #endregion Methods
    }
}