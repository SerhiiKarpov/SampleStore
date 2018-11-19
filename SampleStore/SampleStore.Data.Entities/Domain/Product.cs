namespace SampleStore.Data.Entities.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class encapsulating product.
    /// </summary>
    /// <seealso cref="Entity" />
    public class Product : Entity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the photo identifier.
        /// </summary>
        public Guid? PhotoId
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        [DataType(DataType.Currency)]
        public decimal Price
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        public double Quantity
        {
            get; set;
        }

        #endregion Properties
    }
}