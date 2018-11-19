[assembly: Microsoft.AspNetCore.Hosting.HostingStartup(typeof(SampleStore.UI.Areas.Identity.IdentityHostingStartup))]

namespace SampleStore.UI.Areas.Identity
{
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// Class encapsulating identity hosting startup.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Hosting.IHostingStartup" />
    public class IdentityHostingStartup : IHostingStartup
    {
        #region Methods

        /// <summary>
        /// Configure the <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="builder"></param>
        /// <remarks>
        /// Configure is intended to be called before user code, allowing a user to overwrite any changes made.
        /// </remarks>
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }

        #endregion Methods
    }
}