namespace SampleStore.Services.Identity
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using SampleStore.Common.Extensions;
    using SampleStore.Data.Entities.Identity;
    using SampleStore.Data.Extensions;

    /// <summary>
    /// Class encapsulating user store.
    /// </summary>
    /// <seealso cref="IUserLoginStore{User}" />
    public partial class UserStore : IUserLoginStore<User>
    {
        #region Methods

        /// <summary>
        /// Adds an external <see cref="T:Microsoft.AspNetCore.Identity.UserLoginInfo" /> to the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user to add the login to.</param>
        /// <param name="login">The external <see cref="T:Microsoft.AspNetCore.Identity.UserLoginInfo" /> to add to the specified <paramref name="user" />.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
        /// </returns>
        public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));
            login.ThrowIfArgumentIsNull(nameof(login));

            var mapper = _mapperProvider.GetMapper<UserLoginInfo, UserLogin>();
            var userLogin = mapper.Map(login);
            userLogin.UserId = user.Id;
            _unitOfWork.GetRepository<UserLogin>().Add(userLogin);
            await _unitOfWork.SaveChanges(cancellationToken);
        }

        /// <summary>
        /// Retrieves the user associated with the specified login provider and login provider key.
        /// </summary>
        /// <param name="loginProvider">The login provider who provided the <paramref name="providerKey" />.</param>
        /// <param name="providerKey">The key provided by the <paramref name="loginProvider" /> to identify a user.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> for the asynchronous operation, containing the user, if any which matched the specified login provider and key.
        /// </returns>
        public async Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query =
                from u in _unitOfWork.GetRepository<User>().Query
                where _unitOfWork.GetRepository<UserLogin>().Query.Any(
                    ul => ul.UserId == u.Id
                        && ul.LoginProvider == loginProvider
                        && ul.ProviderKey == providerKey)
                select u;
            var user = await _queryMaterializer.FirstOrDefault(query, cancellationToken);
            return user;
        }

        /// <summary>
        /// Retrieves the associated logins for the specified <param ref="user" />.
        /// </summary>
        /// <param name="user">The user whose associated logins to retrieve.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> for the asynchronous operation, containing a list of <see cref="T:Microsoft.AspNetCore.Identity.UserLoginInfo" /> for the specified <paramref name="user" />, if any.
        /// </returns>
        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));

            var query = _unitOfWork.GetRepository<UserLogin>().Query.Where(ul => ul.UserId == user.Id);
            var userLogins = await _queryMaterializer.ToList(query, cancellationToken);
            var mapper = _mapperProvider.GetMapper<UserLogin, UserLoginInfo>();
            var logins = userLogins.Select(mapper.Map).ToList();
            return logins;
        }

        /// <summary>
        /// Attempts to remove the provided login information from the specified <paramref name="user" />.
        /// and returns a flag indicating whether the removal succeed or not.
        /// </summary>
        /// <param name="user">The user to remove the login information from.</param>
        /// <param name="loginProvider">The login provide whose information should be removed.</param>
        /// <param name="providerKey">The key given by the external login provider for the specified user.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
        /// </returns>
        public async Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));

            var repository = _unitOfWork.GetRepository<UserLogin>();

            var query =
                from ul in repository.Query
                where ul.UserId == user.Id
                    && ul.LoginProvider == loginProvider
                    && ul.ProviderKey == providerKey
                select ul;

            var userLogin = await _queryMaterializer.FirstOrDefault(query, cancellationToken);
            if (userLogin == null)
            {
                return;
            }

            repository.Remove(userLogin);
            await _unitOfWork.SaveChanges(cancellationToken);
        }

        #endregion Methods
    }
}