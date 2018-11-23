namespace SampleStore.Common.Commands
{
    using System.Threading.Tasks;

    /// <summary>
    /// An interface for command.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface ICommand<TResult>
    {
        #region Methods

        /// <summary>
        /// Executes the command encapsulated by this instance.
        /// </summary>
        /// <returns>
        /// The command result.
        /// </returns>
        Task<TResult> Do();

        #endregion Methods
    }
}