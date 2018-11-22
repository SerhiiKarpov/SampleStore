namespace SampleStore.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class encapsulating entity.
    /// </summary>
    public abstract class Entity
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [Key]
        public Guid Id
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        [Timestamp]
#pragma warning disable CA1819 // Properties should not return arrays
        public byte[] RowVersion
#pragma warning restore CA1819 // Properties should not return arrays
        {
            get; set;
        }

        #endregion Properties
    }
}