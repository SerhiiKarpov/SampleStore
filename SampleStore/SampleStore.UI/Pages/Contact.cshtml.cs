namespace SampleStore.UI.Pages
{
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// Class encapsulating contact model.
    /// </summary>
    /// <seealso cref="PageModelBase" />
    [AllowAnonymous]
    public class ContactModel : PageModelBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message
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
                return "Contact";
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Called when [get].
        /// </summary>
        public void OnGet()
        {
            Message = "Your contact page.";
        }

        #endregion Methods
    }
}