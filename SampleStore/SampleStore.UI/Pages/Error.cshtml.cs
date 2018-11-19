namespace SampleStore.UI.Pages
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Class encapsulating error model.
    /// </summary>
    /// <seealso cref="PageModelBase" />
    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModelBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the request identifier.
        /// </summary>
        public string RequestId
        {
            get; set;
        }

        /// <summary>
        /// Gets a value indicating whether [show request identifier].
        /// </summary>
        public bool ShowRequestId
        {
            get
            {
                return !string.IsNullOrEmpty(RequestId);
            }
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        public override string Title
        {
            get
            {
                return "Error";
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Called when [get].
        /// </summary>
        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }

        #endregion Methods
    }
}