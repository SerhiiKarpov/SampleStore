namespace SampleStore.Host.Configuration;

public class ExternalLoginOptions
{
    public string CallbackPath { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;
}