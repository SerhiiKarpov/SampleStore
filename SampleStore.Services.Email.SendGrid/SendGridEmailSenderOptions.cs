namespace SampleStore.Services.Email.SendGrid
{
    /// <summary>
    /// Class encapsulating send grid email sender options.
    /// </summary>
    public class SendGridEmailSenderOptions
    {
        #region Properties

        /// <summary>
        /// Gets or sets the send grid key.
        /// </summary>
        /// <value>
        /// The send grid key.
        /// </value>
        public string SendGridKey
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the send grid user.
        /// </summary>
        /// <value>
        /// The send grid user.
        /// </value>
        public string SendGridUser
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the sender email.
        /// </summary>
        /// <value>
        /// The sender email.
        /// </value>
        public string SenderEmail
        {
            get; set;
        }

        #endregion Properties
    }
}