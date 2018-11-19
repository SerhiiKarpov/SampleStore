namespace SampleStore.UI.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using SampleStore.Common;
    using SampleStore.Data.Entities.Identity;
    using SampleStore.UI.Pages;

    /// <summary>
    /// Class encapsulating external logins model.
    /// </summary>
    /// <seealso cref="PageModelBase" />
    public class ExternalLoginsModel : PageModelBase
    {
        #region Fields

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
        /// Initializes a new instance of the <see cref="ExternalLoginsModel"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign in manager.</param>
        public ExternalLoginsModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
            _signInManager = signInManager.ThrowIfArgumentIsNull(nameof(signInManager));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the current logins.
        /// </summary>
        /// <value>
        /// The current logins.
        /// </value>
        public IList<UserLoginInfo> CurrentLogins
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the other logins.
        /// </summary>
        /// <value>
        /// The other logins.
        /// </value>
        public IList<AuthenticationScheme> OtherLogins
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show remove button].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show remove button]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowRemoveButton
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        /// <value>
        /// The status message.
        /// </value>
        [TempData]
        public string StatusMessage
        {
            get; set;
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title
        {
            get
            {
                return "Manage your external logins";
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Called when get asynchronous.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            CurrentLogins = await _userManager.GetLoginsAsync(user);
            OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                .Where(auth => CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
                .ToList();
            ShowRemoveButton = user.PasswordHash != null || CurrentLogins.Count > 1;
            return Page();
        }

        /// <summary>
        /// Called when get link login callback asynchronous.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Unexpected error occurred loading external login info for user with ID '{user.Id}
        /// or
        /// Unexpected error occurred adding external login for user with ID '{user.Id}
        /// </exception>
        public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync(await _userManager.GetUserIdAsync(user));
            if (info == null)
            {
                throw new InvalidOperationException($"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");
            }

            var result = await _userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred adding external login for user with ID '{user.Id}'.");
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            StatusMessage = "The external login was added.";
            return RedirectToPage();
        }

        /// <summary>
        /// Called when post link login asynchronous.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Page("./ExternalLogins", pageHandler: "LinkLoginCallback");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return new ChallengeResult(provider, properties);
        }

        /// <summary>
        /// Called when post remove login asynchronous.
        /// </summary>
        /// <param name="loginProvider">The login provider.</param>
        /// <param name="providerKey">The provider key.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        /// <exception cref="InvalidOperationException">Unexpected error occurred removing external login for user with ID.</exception>
        public async Task<IActionResult> OnPostRemoveLoginAsync(string loginProvider, string providerKey)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
            if (!result.Succeeded)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                throw new InvalidOperationException($"Unexpected error occurred removing external login for user with ID '{userId}'.");
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "The external login was removed.";
            return RedirectToPage();
        }

        #endregion Methods
    }
}