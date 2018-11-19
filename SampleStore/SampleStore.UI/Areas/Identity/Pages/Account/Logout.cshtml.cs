namespace SampleStore.UI.Areas.Identity.Pages.Account
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using SampleStore.Common;
    using SampleStore.Data.Entities.Identity;
    using SampleStore.UI.Pages;

    /// <summary>
    /// Class encapsulating logout model.
    /// </summary>
    /// <seealso cref="PageModelBase" />
    [AllowAnonymous]
    public class LogoutModel : PageModelBase
    {
        #region Fields

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<LogoutModel> _logger;

        /// <summary>
        /// The sign in manager
        /// </summary>
        private readonly SignInManager<User> _signInManager;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogoutModel"/> class.
        /// </summary>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="logger">The logger.</param>
        public LogoutModel(SignInManager<User> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager.ThrowIfArgumentIsNull(nameof(signInManager));
            _logger = logger.ThrowIfArgumentIsNull(nameof(logger));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the title.
        /// </summary>
        public override string Title
        {
            get
            {
                return "Log out";
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Called when [get].
        /// </summary>
#pragma warning disable CA1822 // Mark members as static
        public void OnGet()
#pragma warning restore CA1822 // Mark members as static
        {
        }

        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return Page();
            }
        }

        #endregion Methods
    }
}