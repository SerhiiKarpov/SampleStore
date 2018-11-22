namespace SampleStore.Services.Identity
{
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using SampleStore.Common;
    using SampleStore.Data.Entities.Identity;
    using SampleStore.Data.Extensions;

    /// <summary>
    /// Class encapsulating user store.
    /// </summary>
    /// <seealso cref="IUserAuthenticationTokenStore{User}" />
    public partial class UserStore : IUserAuthenticationTokenStore<User>
    {
        #region Methods

        /// <summary>
        /// Returns the token value.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="loginProvider">The authentication provider for the token.</param>
        /// <param name="name">The name of the token.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
        /// </returns>
        public async Task<string> GetTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));
            var token = await _unitOfWork.GetRepository<UserToken>().Find(
                GetUserTokenPredicate(user.Id, loginProvider, name),
                _queryMaterializer,
                cancellationToken);
            return token?.Value;
        }

        /// <summary>
        /// Deletes a token for a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="loginProvider">The authentication provider for the token.</param>
        /// <param name="name">The name of the token.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
        /// </returns>
        public async Task RemoveTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));

            var repository = _unitOfWork.GetRepository<UserToken>();

            var token = await repository.Find(GetUserTokenPredicate(user.Id, loginProvider, name), _queryMaterializer, cancellationToken);
            if (token == null)
            {
                return;
            }

            repository.Remove(token);
            await _unitOfWork.SaveChanges(cancellationToken);
        }

        /// <summary>
        /// Sets the token value for a particular user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="loginProvider">The authentication provider for the token.</param>
        /// <param name="name">The name of the token.</param>
        /// <param name="value">The value of the token.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
        /// </returns>
        public async Task SetTokenAsync(User user, string loginProvider, string name, string value, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));

            var repository = _unitOfWork.GetRepository<UserToken>();

            var token = await repository.Find(
                GetUserTokenPredicate(user.Id, loginProvider, name),
                _queryMaterializer,
                cancellationToken);

            if (token == null)
            {
                token = new UserToken { UserId = user.Id, LoginProvider = loginProvider, Name = name };
                repository.Add(token);
            }

            token.Value = value;

            await _unitOfWork.SaveChanges(cancellationToken);
        }

        /// <summary>
        /// Gets the user token predicate.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="loginProvider">The login provider.</param>
        /// <param name="name">The name.</param>
        /// <returns>The user token predicate.</returns>
        private static Expression<Func<UserToken, bool>> GetUserTokenPredicate(Guid userId, string loginProvider, string name)
        {
            return ut => ut.UserId == userId
                && ut.LoginProvider == loginProvider
                && ut.Name == name;
        }

        #endregion Methods
    }
}