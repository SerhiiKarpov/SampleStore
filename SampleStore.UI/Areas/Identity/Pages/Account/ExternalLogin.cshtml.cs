namespace SampleStore.UI.Areas.Identity.Pages.Account
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using SampleStore.Common;
    using SampleStore.Data.Entities.Identity;
    using SampleStore.UI.Pages;
    using SampleStore.UI.ViewModels.Identity;

    /// <summary>
    /// Class encapsulating external login model.
    /// </summary>
    /// <seealso cref="PageModelBase" />
    [AllowAnonymous]
    public class ExternalLoginModel : PageModelBase
    {
        #region Fields

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<ExternalLoginModel> _logger;

        /// <summary>
        /// The sign in manager
        /// </summary>
        private readonly SignInManager<User> _signInManager;

        /// <summary>
        /// The user manager
        /// </summary>
        private readonly UserManager<User> _userManager;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalLoginModel"/> class.
        /// </summary>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="userManager">The user manager.</param>
        /// <param name="logger">The logger.</param>
        public ExternalLoginModel(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            ILogger<ExternalLoginModel> logger)
        {
            _signInManager = signInManager.ThrowIfArgumentIsNull(nameof(signInManager));
            _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
            _logger = logger.ThrowIfArgumentIsNull(nameof(logger));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        [TempData]
        public string ErrorMessage
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        [BindProperty]
        public ExternalLoginViewModel Input
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the login provider.
        /// </summary>
        public string LoginProvider
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the return URL.
        /// </summary>
        public string ReturnUrl
        {
            get; set;
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        public override string Title
        {
            get
            {
                return "Register";
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Called when [get asynchronous].
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        /// <summary>
        /// Called when [get callback asynchronous].
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="remoteError">The remote error.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                LoginProvider = info.LoginProvider;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new ExternalLoginViewModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return Page();
            }
        }

        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        /// <summary>
        /// Called when [post confirmation asynchronous].
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = new User { Email = Input.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            LoginProvider = info.LoginProvider;
            ReturnUrl = returnUrl;
            return Page();
        }

        #endregion Methods
    }
}