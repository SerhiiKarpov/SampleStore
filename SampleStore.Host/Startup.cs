namespace SampleStore.Host
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using SampleStore.Common;
    using SampleStore.Data.EF.Extensions;
    using SampleStore.Data.Seed.Extensions;
    using SampleStore.Host.Configuration;
    using SampleStore.Mapping.AutoMapper.Extensions;
    using SampleStore.Mapping.DataToServices.AutoMapper.Extensions;
    using SampleStore.Mapping.DataToUI.Extensions;
    using SampleStore.Mapping.ServicesToUI.Extensions;
    using SampleStore.Services.Email.SendGrid.Extensions;
    using SampleStore.Services.Identity.Extensions;
    using SampleStore.UI.Extensions;

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
        /// The email sender sender email key
        /// </summary>
        private const string EmailSenderSenderEmailKey = "EmailSender:SenderEmail";

        /// <summary>
        /// The facebook options key
        /// </summary>
        private const string FacebookOptionsKey = "Authentication:Facebook";

        /// <summary>
        /// The google options key
        /// </summary>
        private const string GoogleOptionsKey = "Authentication:Google";

        /// <summary>
        /// The identity secion key
        /// </summary>
        private const string IdentitySecionKey = "Identity";

        /// <summary>
        /// The microsoft options key
        /// </summary>
        private const string MicrosoftOptionsKey = "Authentication:Microsoft";

        /// <summary>
        /// The send grid key key
        /// </summary>
        private const string SendGridKeyKey = "EmailSender:SendGrid:Key";

        /// <summary>
        /// The send grid user key
        /// </summary>
        private const string SendGridUserKey = "EmailSender:SendGrid:User";

        /// <summary>
        /// The twitter options key
        /// </summary>
        private const string TwitterOptionsKey = "Authentication:Twitter";

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

            // TODO: Sort this out.
            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.AddEntityFrameworkDataAccess(Configuration.GetConnectionString(ConnectionStringKey));
            services.AddDatabaseSeeder();

            services.AddAutoMapper(
                options => options
                    .AddDataToServicesMappings()
                    .AddDataToUIMappings()
                    .AddServicesToUIMappings());

            services.AddCustomizedIdentity();
            services.AddAuthentication()
                .AddFacebook(
                    options =>
                    {
                        var elo = Configuration.GetSection(FacebookOptionsKey).Get<ExternalLoginOptions>();
                        options.AppId = elo.ClientId;
                        options.AppSecret = elo.ClientSecret;
                        options.CallbackPath = elo.CallbackPath;

                        // NOTE: If you need more fields, please see https://developers.facebook.com/docs/graph-api/reference/user
                        options.Fields.Add("first_name", "last_name", "email", "birthday");

                        // NOTE: For more scopes, please check User Data section here: https://developers.facebook.com/docs/facebook-login/permissions/
                        options.Scope.Add("public_profile", "email", "user_birthday");
                    })
                .AddTwitter(
                    options =>
                    {
                        var elo = Configuration.GetSection(TwitterOptionsKey).Get<ExternalLoginOptions>();
                        options.ConsumerKey = elo.ClientId;
                        options.ConsumerSecret = elo.ClientSecret;
                        options.CallbackPath = elo.CallbackPath;

                        // NOTE: Email address is not provided by Twitter API as of the moment.
                    })
                .AddGoogle(
                    options =>
                    {
                        var elo = Configuration.GetSection(GoogleOptionsKey).Get<ExternalLoginOptions>();
                        options.ClientId = elo.ClientId;
                        options.ClientSecret = elo.ClientSecret;
                        options.CallbackPath = elo.CallbackPath;

                        // NOTE: The following code is to retrieve birthday information.
                        // Found the details at these URLs:
                        // 1) https://developers.google.com/+/web/api/rest/oauth#plus.login
                        // 2) https://github.com/aspnet/Security/issues/1728
                        //options.Scope.Add("https://www.googleapis.com/auth/plus.login");
                        options.ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, "birthday");
                    })
                .AddMicrosoftAccount(
                    options =>
                    {
                        var elo = Configuration.GetSection(MicrosoftOptionsKey).Get<ExternalLoginOptions>();
                        options.ClientId = elo.ClientId;
                        options.ClientSecret = elo.ClientSecret;
                        options.CallbackPath = elo.CallbackPath;

                        // TODO: Find out how to get birthday from Microsoft Account.
                        //options.ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, "birthday");
                    });

            services.AddSendGridEmailSender(
                options =>
                {
                    options.SenderEmail = Configuration[EmailSenderSenderEmailKey];
                    options.SendGridUser = Configuration[SendGridUserKey];
                    options.SendGridKey = Configuration[SendGridKeyKey];
                });

            services.Configure<IdentityOptions>(Configuration.GetSection(IdentitySecionKey));
            services.Configure<CookieAuthenticationOptions>(
                IdentityConstants.ApplicationScheme,
                Configuration.GetSection(CookieAuthenticationSectionKey));

            services.ConfigureUI();

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