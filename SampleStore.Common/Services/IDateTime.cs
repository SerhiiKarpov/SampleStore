namespace SampleStore.Common.Services
{
    using System;

    /// <summary>
    /// An interface for date time.
    /// </summary>
    public interface IDateTime
    {
        #region Properties

        /// <summary>
        /// Gets the UTC now.
        /// </summary>
        /// <value>
        /// The UTC now.
        /// </value>
        DateTime UtcNow
        {
            get;
        }

        #endregion Properties
    }
}