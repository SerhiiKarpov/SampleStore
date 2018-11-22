namespace SampleStore.Data.Entities.Identity
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class encapsulating role.
    /// </summary>
    /// <seealso cref="Entity" />
    public class Role : Entity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name
        {
            get; set;
        }

        #endregion Properties
    }
}