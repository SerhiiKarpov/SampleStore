namespace SampleStore.UI.Configuration
{
    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Options;

    using SampleStore.Common;

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
        private readonly IHostingEnvironment _environment;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RclStaticFileOptions"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public RclStaticFileOptions(IHostingEnvironment environment)
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
        /// <exception cref="InvalidOperationException">Missing FileProvider.</exception>
        public void PostConfigure(string name, StaticFileOptions options)
        {
            name.ThrowIfArgumentIsNull(nameof(name));
            options.ThrowIfArgumentIsNull(nameof(options));

            // Basic initialization in case the options weren't initialized by any other component
            options.ContentTypeProvider = options.ContentTypeProvider ?? new FileExtensionContentTypeProvider();
            var baseFileProvider = options.FileProvider
                ?? _environment.WebRootFileProvider
                ?? throw new InvalidOperationException("Missing FileProvider.");

            var basePath = "wwwroot";

            var manifestEmbeddedFileProvider = new ManifestEmbeddedFileProvider(GetType().Assembly, basePath);
            options.FileProvider = new CompositeFileProvider(baseFileProvider, manifestEmbeddedFileProvider);
        }

        #endregion Methods
    }
}