namespace SampleStore.Data.Entities.Domain
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class encapsulating photo.
    /// </summary>
    /// <seealso cref="Entity" />
    public class Photo : Entity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
        public byte[] Image
#pragma warning restore CA1819 // Properties should not return arrays
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the type of the MIME.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string MimeType
        {
            get; set;
        }

        #endregion Properties
    }
}