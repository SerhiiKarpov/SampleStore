namespace SampleStore.Data.Entities.Domain
{
    /// <summary>
    /// Class encapsulating photo.
    /// </summary>
    /// <seealso cref="Entity" />
    public class Photo : Entity
    {
        #region Properties

#pragma warning disable CA1819 // Properties should not return arrays
                              /// <summary>
                              /// Gets or sets the image.
                              /// </summary>
        public byte[] Image
#pragma warning restore CA1819 // Properties should not return arrays
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the type of the MIME.
        /// </summary>
        public string MimeType
        {
            get; set;
        }

        #endregion Properties
    }
}