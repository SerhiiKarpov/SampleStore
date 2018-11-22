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
    /// <seealso cref="IUserPasswordStore{User}" />
    public partial class UserStore : IUserPasswordStore<User>
    {
        #region Methods

        /// <summary>
        /// Gets the password hash for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose password hash to retrieve.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, returning the password hash for the specified <paramref name="user" />.
        /// </returns>
        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));
            return Task.FromResult(user.PasswordHash);
        }

        /// <summary>
        /// Gets a flag indicating whether the specified <paramref name="user" /> has a password.
        /// </summary>
        /// <param name="user">The user to return a flag for, indicating whether they have a password or not.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, returning true if the specified <paramref name="user" /> has a password
        /// otherwise false.
        /// </returns>
        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));
            return Task.FromResult(user.PasswordHash != null);
        }

        /// <summary>
        /// Sets the password hash for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose password hash to set.</param>
        /// <param name="passwordHash">The password hash to set.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
        /// </returns>
        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        #endregion Methods
    }
}