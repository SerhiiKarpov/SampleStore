namespace SampleStore.Services.Identity
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using SampleStore.Common;
    using SampleStore.Data.Entities.Identity;

    /// <summary>
    /// Class encapsulating user store.
    /// </summary>
    /// <seealso cref="IUserTwoFactorStore{User}" />
    public partial class UserStore : IUserTwoFactorStore<User>
    {
        #region Methods

        /// <summary>
        /// Returns a flag indicating whether the specified <paramref name="user" /> has two factor authentication enabled or not,
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user whose two factor authentication enabled status should be set.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing a flag indicating whether the specified
        /// <paramref name="user" /> has two factor authentication enabled or not.
        /// </returns>
        public Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));
            return Task.FromResult(user.TwoFactorEnabled);
        }

        /// <summary>
        /// Sets a flag indicating whether the specified <paramref name="user" /> has two factor authentication enabled or not,
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user whose two factor authentication enabled status should be set.</param>
        /// <param name="enabled">A flag indicating whether the specified <paramref name="user" /> has two factor authentication enabled.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
        /// </returns>
        public Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));
            user.TwoFactorEnabled = enabled;
            return Task.CompletedTask;
        }

        #endregion Methods
    }
}