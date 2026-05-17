using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

using SampleStore.Common.Extensions;

namespace SampleStore.UI.Configuration;

public class RclStaticFileOptions : IPostConfigureOptions<StaticFileOptions>
{
    private readonly IWebHostEnvironment _environment;

    public RclStaticFileOptions(IWebHostEnvironment environment)
    {
        _environment = environment.ThrowIfArgumentIsNull(nameof(environment));
    }

    public void PostConfigure(string? name, StaticFileOptions options)
    {
        // Static web assets from RCLs are served automatically by the .NET 10 Razor SDK.
        // The legacy ManifestEmbeddedFileProvider approach has been removed.
    }
}
