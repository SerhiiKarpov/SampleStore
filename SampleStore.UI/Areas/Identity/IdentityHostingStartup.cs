using Microsoft.AspNetCore.Hosting;
[assembly: Microsoft.AspNetCore.Hosting.HostingStartup(typeof(SampleStore.UI.Areas.Identity.IdentityHostingStartup))]

namespace SampleStore.UI.Areas.Identity;

public class IdentityHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) => {
        });
    }
}