namespace SampleStore.UI
{
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using SampleStore.Common;
    using SampleStore.Data.EF.Extensions;
    using SampleStore.Data.Seed.Extensions;
    using SampleStore.Identity.Extensions;
    using SampleStore.UI.Services;

    /// <summary>
    /// Class encapsulating startup.
    /// </summary>
    public class Startup
    {
        #region Fields

        /// <summary>
        /// The connection string key
        /// </summary>
        public const string ConnectionStringKey = "DefaultConnection";

        /// <summary>
        /// The cookie authentication section key
        /// </summary>
        private const string CookieAuthenticationSectionKey = "CookieAuthentication";

        /// <summary>
        /// The data protection token provider section key
        /// </summary>
        private const string DataProtectionTokenProviderSectionKey = "DataProtectionTokenProvider";

        /// <summary>
        /// The identity secion key
        /// </summary>
        private const string IdentitySecionKey = "Identity";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration.ThrowIfArgumentIsNull(nameof(configuration));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        /// <remarks>This method gets called by the runtime. Use this method to configure the HTTP request pipeline.</remarks>
        public static void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <remarks>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </remarks>
        public void ConfigureServices(IServiceCollection services)
        {
            services.ThrowIfArgumentIsNull(nameof(services));

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.AddEntityFrameworkDataAccess(Configuration.GetConnectionString(ConnectionStringKey));
            services.AddDatabaseSeeder();

            services.AddCustomizedIdentity();

            services.AddScoped<IEmailSender, StubEmailSender>();

            services.Configure<IdentityOptions>(Configuration.GetSection(IdentitySecionKey));
            services.Configure<CookieAuthenticationOptions>(
                IdentityConstants.ApplicationScheme,
                Configuration.GetSection(CookieAuthenticationSectionKey));

            services.AddMvc(
                    options =>
                    {
                        var policy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
                        options.Filters.Add(new AuthorizeFilter(policy));
                    })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        #endregion Methods
    }
}