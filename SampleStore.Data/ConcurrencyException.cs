namespace SampleStore.Data
{
    using System;
    using System.Collections;
    using System.Linq;

    using SampleStore.Common.Extensions;

    /// <summary>
    /// Class encapsulating concurrency exception.
    /// </summary>
    /// <seealso cref="Exception" />
    public class ConcurrencyException : Exception
    {
        #region Fields

        /// <summary>
        /// The default message
        /// </summary>
        private const string DefaultMessage = "One or more entities were updated or deleted by other process.";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        public ConcurrencyException(IEnumerable conflictedEntities)
            : this(DefaultMessage)
        {
            ConflictedEntities = conflictedEntities.ThrowIfArgumentIsNull(nameof(conflictedEntities));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ConcurrencyException(string message)
            : base(message)
        {
            ConflictedEntities = Enumerable.Empty<object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {
            ConflictedEntities = Enumerable.Empty<object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        public ConcurrencyException()
            : this(Enumerable.Empty<object>())
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the conflicted entities.
        /// </summary>
        public IEnumerable ConflictedEntities
        {
            get;
        }

        #endregion Properties
    }
}