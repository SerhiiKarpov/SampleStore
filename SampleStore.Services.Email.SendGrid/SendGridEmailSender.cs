using System;
using System.Threading.Tasks;

using SendGrid;
using SendGrid.Helpers.Mail;

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace SampleStore.Services.Email.SendGrid;

internal sealed class SendGridEmailSender(IOptions<SendGridEmailSenderOptions> optionsAccessor) : IEmailSender
{
    private readonly SendGridEmailSenderOptions _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var client = new SendGridClient(_options.ApiKey);
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

        var response = await client.SendEmailAsync(message);
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Body.ReadAsStringAsync();
            throw new Exception($"Failed to send email with code {response.StatusCode}: {content}");
        }
    }
}