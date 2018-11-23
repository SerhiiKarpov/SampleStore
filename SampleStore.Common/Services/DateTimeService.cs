namespace SampleStore.Common.Services
{
    using System;

    /// <summary>
    /// Class encapsulating date time service.
    /// </summary>
    /// <seealso cref="SampleStore.Common.Services.IDateTime" />
    public class DateTimeService : IDateTime
    {
        #region Properties

        /// <summary>
        /// Gets the UTC now.
        /// </summary>
        /// <value>
        /// The UTC now.
        /// </value>
        public DateTime UtcNow
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        #endregion Properties
    }
}