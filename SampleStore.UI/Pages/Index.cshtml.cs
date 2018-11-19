namespace SampleStore.UI.Pages
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    /// <summary>
    /// Class encapsulating index model.
    /// </summary>
    /// <seealso cref="PageModel" />
    [AllowAnonymous]
    public class IndexModel : PageModelBase
    {
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
                return "Home page";
            }
        }

        #endregion Properties
    }
}