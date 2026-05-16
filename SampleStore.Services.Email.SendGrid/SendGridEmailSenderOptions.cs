using System.ComponentModel.DataAnnotations;

namespace SampleStore.Services.Email.SendGrid;

public sealed class SendGridEmailSenderOptions
{
    public const string Key = "SendGrid";

    [Required]
    public string ApiKey { get; set; } = string.Empty;

    [Required]
    public string SenderEmail { get; set; } = string.Empty;
}