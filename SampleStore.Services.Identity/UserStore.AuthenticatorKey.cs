namespace SampleStore.Services.Identity
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using SampleStore.Data.Entities.Identity;

    /// <summary>
    /// Class encapsulating user store.
    /// </summary>
    /// <seealso cref="IUserAuthenticatorKeyStore{User}" />
    public partial class UserStore : IUserAuthenticatorKeyStore<User>
    {
        #region Fields

        /// <summary>
        /// The authenticator key token login provider
        /// </summary>
        private const string AuthenticatorKeyTokenLoginProvider = "AuthenticatorKeyTokenLoginProvider";

        /// <summary>
        /// The authenticator key token name
        /// </summary>
        private const string AuthenticatorKeyTokenName = "AuthenticatorKeyTokenName";

        #endregion Fields

        #region Methods

        /// <summary>
        /// Get the authenticator key for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose security stamp should be set.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the security stamp for the specified <paramref name="user" />.
        /// </returns>
        public Task<string> GetAuthenticatorKeyAsync(User user, CancellationToken cancellationToken)
        {
            return GetTokenAsync(user, AuthenticatorKeyTokenLoginProvider, AuthenticatorKeyTokenName, cancellationToken);
        }

        /// <summary>
        /// Sets the authenticator key for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose authenticator key should be set.</param>
        /// <param name="key">The authenticator key to set.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
        /// </returns>
        public Task SetAuthenticatorKeyAsync(User user, string key, CancellationToken cancellationToken)
        {
            return SetTokenAsync(user, AuthenticatorKeyTokenLoginProvider, AuthenticatorKeyTokenName, key, cancellationToken);
        }

        #endregion Methods
    }
}