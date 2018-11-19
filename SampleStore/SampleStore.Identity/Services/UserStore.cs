namespace SampleStore.Identity.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using SampleStore.Common;
    using SampleStore.Data;
    using SampleStore.Data.Entities.Identity;
    using SampleStore.Data.Extensions;

    /// <summary>
    /// Class encapsulating user store.
    /// </summary>
    /// <seealso cref="IUserStore{User}" />
    public partial class UserStore : Disposable, IUserStore<User>
    {
        #region Fields

        /// <summary>
        /// The query materializer
        /// </summary>
        private readonly IQueryMaterializer _queryMaterializer;

        /// <summary>
        /// The role store
        /// </summary>
        private readonly IRoleStore<Role> _roleStore;

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserStore" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="queryMaterializer">The query materializer.</param>
        /// <param name="roleStore">The role store.</param>
        public UserStore(IUnitOfWork unitOfWork, IQueryMaterializer queryMaterializer, IRoleStore<Role> roleStore)
        {
            _unitOfWork = unitOfWork.ThrowIfArgumentIsNull(nameof(unitOfWork));
            _queryMaterializer = queryMaterializer.ThrowIfArgumentIsNull(nameof(queryMaterializer));
            _roleStore = roleStore.ThrowIfArgumentIsNull(nameof(roleStore));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates the specified <paramref name="user" /> in the user store.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the creation operation.
        /// </returns>
        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));

            _unitOfWork.GetRepository<User>().Add(user);
            await _unitOfWork.SaveChanges(cancellationToken);

            return IdentityResult.Success;
        }

        /// <summary>
        /// Deletes the specified <paramref name="user" /> from the user store.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the update operation.
        /// </returns>
        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user != null)
            {
                _unitOfWork.GetRepository<User>().Remove(user);
                await _unitOfWork.SaveChanges(cancellationToken);
            }

            return IdentityResult.Success;
        }

        /// <summary>
        /// Finds and returns a user, if any, who has the specified <paramref name="userId" />.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the user matching the specified <paramref name="userId" /> if it exists.
        /// </returns>
        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(userId, out var id))
            {
                return null;
            }

            var user = await _unitOfWork.GetRepository<User>().FindById(id, _queryMaterializer, cancellationToken);
            return user;
        }

        /// <summary>
        /// Finds and returns a user, if any, who has the specified normalized user name.
        /// </summary>
        /// <param name="normalizedUserName">The normalized user name to search for.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the user matching the specified <paramref name="normalizedUserName" /> if it exists.
        /// </returns>
        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _unitOfWork.GetRepository<User>().Find(u => u.Email == normalizedUserName, _queryMaterializer, cancellationToken);
            return user;
        }

        /// <summary>
        /// Gets the normalized user name for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose normalized name should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the normalized user name for the specified <paramref name="user" />.
        /// </returns>
        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return GetUserNameAsync(user, cancellationToken);
        }

        /// <summary>
        /// Gets the user identifier for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose identifier should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the identifier for the specified <paramref name="user" />.
        /// </returns>
        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));
            return Task.FromResult(user.Id.ToString());
        }

        /// <summary>
        /// Gets the user name for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose name should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the name for the specified <paramref name="user" />.
        /// </returns>
        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return GetEmailAsync(user, cancellationToken);
        }

        /// <summary>
        /// Sets the given normalized name for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose name should be set.</param>
        /// <param name="normalizedName">The normalized name to set.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
        /// </returns>
        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            return SetUserNameAsync(user, normalizedName, cancellationToken);
        }

        /// <summary>
        /// Sets the given <paramref name="userName" /> for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose name should be set.</param>
        /// <param name="userName">The user name to set.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task" /> that represents the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">user</exception>
        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            return SetEmailAsync(user, userName, cancellationToken);
        }

        /// <summary>
        /// Updates the specified <paramref name="user" /> in the user store.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task" /> that represents the asynchronous operation, containing the <see cref="IdentityResult" /> of the update operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">user</exception>
        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));

            await _unitOfWork.GetRepository<User>().Update(user, _queryMaterializer, cancellationToken);
            return IdentityResult.Success;
        }

        #endregion Methods
    }
}