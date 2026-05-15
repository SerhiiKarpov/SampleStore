namespace SampleStore.UI.Configuration
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Options;

    using SampleStore.Common.Extensions;

    /// <summary>
    /// Class encapsulating RCL static file options.
    /// </summary>
    /// <seealso cref="IPostConfigureOptions{StaticFileOptions}" />
    public class RclStaticFileOptions : IPostConfigureOptions<StaticFileOptions>
    {
        #region Fields

        /// <summary>
        /// The environment
        /// </summary>
        private readonly IWebHostEnvironment _environment;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RclStaticFileOptions"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public RclStaticFileOptions(IWebHostEnvironment environment)
        {
            _environment = environment.ThrowIfArgumentIsNull(nameof(environment));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Invoked to configure a TOptions instance.
        /// </summary>
        /// <param name="name">The name of the options instance being configured.</param>
        /// <param name="options">The options instance to configured.</param>
        public void PostConfigure(string? name, StaticFileOptions options)
        {
            // Static web assets from RCLs are served automatically by the .NET 10 Razor SDK.
            // The legacy ManifestEmbeddedFileProvider approach has been removed.
        }

        #endregion Methods
    }
}
