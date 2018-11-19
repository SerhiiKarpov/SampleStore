namespace SampleStore.UI
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    using SampleStore.UI.Extensions;

    /// <summary>
    /// Class encapsulating program.
    /// </summary>
    public static class Program
    {
        #region Methods

        /// <summary>
        /// Creates the web host builder.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().EnsureSeeded().Run();
        }

        #endregion Methods
    }
}