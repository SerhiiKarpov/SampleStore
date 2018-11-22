namespace SampleStore.Host.Configuration
{
    /// <summary>
    /// Class encapsulating external login options.
    /// </summary>
    public class ExternalLoginOptions
    {
        #region Properties

        /// <summary>
        /// Gets or sets the callback path.
        /// </summary>
        /// <value>
        /// The callback path.
        /// </value>
        public string CallbackPath
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        public string ClientSecret
        {
            get; set;
        }

        #endregion Properties
    }
}