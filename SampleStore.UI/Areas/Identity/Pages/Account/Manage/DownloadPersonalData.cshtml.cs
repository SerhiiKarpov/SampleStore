namespace SampleStore.UI.Areas.Identity.Pages.Account.Manage
{
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;

    using SampleStore.Common.Extensions;
    using SampleStore.Data.Entities.Identity;
    using SampleStore.UI.Pages;

    /// <summary>
    /// Class encapsulating download personal data model.
    /// </summary>
    /// <seealso cref="PageModelBase" />
    public class DownloadPersonalDataModel : PageModelBase
    {
        #region Fields

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<DownloadPersonalDataModel> _logger;

        /// <summary>
        /// The user manager
        /// </summary>
        private readonly UserManager<User> _userManager;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadPersonalDataModel"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="logger">The logger.</param>
        public DownloadPersonalDataModel(
            UserManager<User> userManager,
            ILogger<DownloadPersonalDataModel> logger)
        {
            _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
            _logger = logger.ThrowIfArgumentIsNull(nameof(logger));
        }

        #endregion Constructors

        #region Properties

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
                return "Download Your Data";
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Called when post asynchronous.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("User with ID '{UserId}' asked for their personal data.", _userManager.GetUserId(User));

            // Only include personal data for download
            var personalData = new Dictionary<string, string>
            {
                { "Id", user.Id.ToString() },
                { "FullName", user.FullName },
                { "DateOfBirth", user.DateOfBirth.ToString() },
                { "Email", user.Email },
                { "PhoneNumber", user.PhoneNumber }
            };

            Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
            return new FileContentResult(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(personalData)), "text/json");
        }

        #endregion Methods
    }
}