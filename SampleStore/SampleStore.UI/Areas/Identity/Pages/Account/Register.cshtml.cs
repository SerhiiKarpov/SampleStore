namespace SampleStore.UI.Areas.Identity.Pages.Account
{
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using SampleStore.Common;
    using SampleStore.Data.Entities.Identity;
    using SampleStore.UI.Pages;
    using SampleStore.UI.ViewModels.Identity;

    /// <summary>
    /// Class encapsulating register model.
    /// </summary>
    /// <seealso cref="PageModelBase" />
    [AllowAnonymous]
    public class RegisterModel : PageModelBase
    {
        #region Fields

        /// <summary>
        /// The email sender
        /// </summary>
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<RegisterModel> _logger;

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
        /// Initializes a new instance of the <see cref="RegisterModel"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="emailSender">The email sender.</param>
        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
            _signInManager = signInManager.ThrowIfArgumentIsNull(nameof(signInManager));
            _logger = logger.ThrowIfArgumentIsNull(nameof(logger));
            _emailSender = emailSender.ThrowIfArgumentIsNull(nameof(emailSender));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        [BindProperty]
        public RegistrationViewModel Input
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
        /// Called when [get].
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        /// <summary>
        /// Called when [post asynchronous].
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new User
            {
                FullName = Input.Name,
                DateOfBirth = Input.DateOfBirth,
                Email = Input.Email
            };
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            _logger.LogInformation("User created a new account with password.");

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = user.Id, code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
        }

        #endregion Methods
    }
}