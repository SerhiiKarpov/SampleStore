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
    /// Class encapsulating role store.
    /// </summary>
    /// <seealso cref="IRoleStore{Role}" />
    public partial class RoleStore : Disposable, IRoleStore<Role>
    {
        #region Fields

        /// <summary>
        /// The query materializer
        /// </summary>
        private readonly IQueryMaterializer _queryMaterializer;

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleStore"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="queryMaterializer">The query materializer.</param>
        public RoleStore(IUnitOfWork unitOfWork, IQueryMaterializer queryMaterializer)
        {
            _unitOfWork = unitOfWork.ThrowIfArgumentIsNull(nameof(unitOfWork));
            _queryMaterializer = queryMaterializer.ThrowIfArgumentIsNull(nameof(queryMaterializer));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates a new role in a store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to create in the store.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the asynchronous query.
        /// </returns>
        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfArgumentIsNull(nameof(role));

            _unitOfWork.GetRepository<Role>().Add(role);
            await _unitOfWork.SaveChanges(cancellationToken);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Deletes a role from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to delete from the store.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the asynchronous query.
        /// </returns>
        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfArgumentIsNull(nameof(role));

            _unitOfWork.GetRepository<Role>().Remove(role);
            await _unitOfWork.SaveChanges(cancellationToken);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Finds the role who has the specified ID as an asynchronous operation.
        /// </summary>
        /// <param name="roleId">The role ID to look for.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task`1" /> that result of the look up.
        /// </returns>
        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(roleId, out var id))
            {
                return null;
            }

            var role = await _unitOfWork.GetRepository<Role>().FindById(id, _queryMaterializer, cancellationToken);
            return role;
        }

        /// <summary>
        /// Finds the role who has the specified normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="normalizedRoleName">The normalized role name to look for.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task`1" /> that result of the look up.
        /// </returns>
        public Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return _unitOfWork.GetRepository<Role>().Find(r => r.Name == normalizedRoleName, _queryMaterializer, cancellationToken);
        }

        /// <summary>
        /// Get a role's normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose normalized name should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task`1" /> that contains the name of the role.
        /// </returns>
        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfArgumentIsNull(nameof(role));
            return Task.FromResult(role.Name);
        }

        /// <summary>
        /// Gets the ID for a role from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose ID should be returned.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task`1" /> that contains the ID of the role.
        /// </returns>
        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfArgumentIsNull(nameof(role));
            return Task.FromResult(role.Id.ToString());
        }

        /// <summary>
        /// Gets the name of a role from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose name should be returned.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task`1" /> that contains the name of the role.
        /// </returns>
        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfArgumentIsNull(nameof(role));
            return Task.FromResult(role.Name);
        }

        /// <summary>
        /// Set a role's normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose normalized name should be set.</param>
        /// <param name="normalizedName">The normalized name to set</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
        /// </returns>
        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            return SetRoleNameAsync(role, normalizedName, cancellationToken);
        }

        /// <summary>
        /// Sets the name of a role in the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose name should be set.</param>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
        /// </returns>
        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfArgumentIsNull(nameof(role));
            role.Name = roleName;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Updates a role in a store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to update in the store.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the asynchronous query.
        /// </returns>
        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfArgumentIsNull(nameof(role));

            await _unitOfWork.GetRepository<Role>().Update(role, _queryMaterializer, cancellationToken);
            return IdentityResult.Success;
        }

        #endregion Methods
    }
}