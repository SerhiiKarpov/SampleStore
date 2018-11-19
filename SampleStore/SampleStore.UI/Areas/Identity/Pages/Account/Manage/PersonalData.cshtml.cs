namespace SampleStore.UI.Areas.Identity.Pages.Account.Manage
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using SampleStore.Common;
    using SampleStore.Data.Entities.Identity;
    using SampleStore.UI.Pages;

    /// <summary>
    /// Class encapsulating personal data model.
    /// </summary>
    /// <seealso cref="PageModelBase" />
    public class PersonalDataModel : PageModelBase
    {
        #region Fields

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<PersonalDataModel> _logger;

        /// <summary>
        /// The user manager
        /// </summary>
        private readonly UserManager<User> _userManager;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonalDataModel"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="logger">The logger.</param>
        public PersonalDataModel(
            UserManager<User> userManager,
            ILogger<PersonalDataModel> logger)
        {
            _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
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
                return "Personal Data";
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Called when [get].
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }

        #endregion Methods
    }
}