namespace SampleStore.UI.Areas.Identity.Pages.Account.Manage
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using SampleStore.Common.Extensions;
    using SampleStore.Data.Entities.Identity;
    using SampleStore.UI.Pages;

    /// <summary>
    /// Class encapsulating two factor authentication model.
    /// </summary>
    /// <seealso cref="PageModelBase" />
    public class TwoFactorAuthenticationModel : PageModelBase
    {
        #region Fields

        /// <summary>
        /// The authenicator URI format
        /// </summary>
        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}";

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<TwoFactorAuthenticationModel> _logger;

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
        /// Initializes a new instance of the <see cref="TwoFactorAuthenticationModel"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="logger">The logger.</param>
        public TwoFactorAuthenticationModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<TwoFactorAuthenticationModel> logger)
        {
            _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
            _signInManager = signInManager.ThrowIfArgumentIsNull(nameof(signInManager));
            _logger = logger.ThrowIfArgumentIsNull(nameof(logger));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance has authenticator.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has authenticator; otherwise, <c>false</c>.
        /// </value>
        public bool HasAuthenticator
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [is2fa enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is2fa enabled]; otherwise, <c>false</c>.
        /// </value>
        [BindProperty]
        public bool Is2faEnabled
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is machine remembered.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is machine remembered; otherwise, <c>false</c>.
        /// </value>
        public bool IsMachineRemembered
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the recovery codes left.
        /// </summary>
        /// <value>
        /// The recovery codes left.
        /// </value>
        public int RecoveryCodesLeft
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        [TempData]
        public string StatusMessage
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
                return "Two-factor authentication (2FA)";
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Called when get.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null;
            Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user);
            RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user);

            return Page();
        }

        /// <summary>
        /// Called when post.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPost()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await _signInManager.ForgetTwoFactorClientAsync();
            StatusMessage = "The current browser has been forgotten. When you login again from this browser you will be prompted for your 2fa code.";
            return RedirectToPage();
        }

        #endregion Methods
    }
}