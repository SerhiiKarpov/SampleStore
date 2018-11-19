namespace SampleStore.Identity.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
    /// <seealso cref="IUserRoleStore{User}" />
    public partial class UserStore : IUserRoleStore<User>
    {
        #region Methods

        /// <summary>
        /// Add the specified <paramref name="user" /> to the named role.
        /// </summary>
        /// <param name="user">The user to add to the named role.</param>
        /// <param name="roleName">The name of the role to add the user to.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
        /// </returns>
        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));
            roleName.ThrowIfArgumentIsNull(nameof(roleName));

            var role = await _roleStore.FindByNameAsync(roleName, cancellationToken);
            if (role == null)
            {
                return;
            }

            var userRole = new UserRole { UserId = user.Id, RoleId = role.Id };
            _unitOfWork.GetRepository<UserRole>().Add(userRole);
            await _unitOfWork.SaveChanges(cancellationToken);
        }

        /// <summary>
        /// Gets a list of role names the specified <paramref name="user" /> belongs to.
        /// </summary>
        /// <param name="user">The user whose role names to retrieve.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing a list of role names.
        /// </returns>
        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));

            var roleQuery =
                from userRole in _unitOfWork.GetRepository<UserRole>().Query
                join role in _unitOfWork.GetRepository<Role>().Query on userRole.RoleId equals role.Id
                where userRole.UserId == user.Id
                select role;
            var roles = await _queryMaterializer.ToList(roleQuery, cancellationToken);

            var roleNames = new List<string>();
            foreach (var role in roles)
            {
                var roleName = await _roleStore.GetRoleNameAsync(role, cancellationToken);
                roleNames.Add(roleName);
            }

            return roleNames;
        }

        /// <summary>
        /// Returns a list of Users who are members of the named role.
        /// </summary>
        /// <param name="roleName">The name of the role whose membership should be returned.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing a list of users who are in the named role.
        /// </returns>
        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrEmpty(roleName))
            {
                return new List<User>();
            }

            var role = await _roleStore.FindByNameAsync(roleName, cancellationToken);
            if (role == null)
            {
                return new List<User>();
            }

            var userQuery =
                from userRole in _unitOfWork.GetRepository<UserRole>().Query
                join user in _unitOfWork.GetRepository<User>().Query on userRole.UserId equals user.Id
                where userRole.RoleId == role.Id
                select user;
            var users = await _queryMaterializer.ToList(userQuery, cancellationToken);
            return users;
        }

        /// <summary>
        /// Returns a flag indicating whether the specified <paramref name="user" /> is a member of the given named role.
        /// </summary>
        /// <param name="user">The user whose role membership should be checked.</param>
        /// <param name="roleName">The name of the role to be checked.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing a flag indicating whether the specified <paramref name="user" /> is
        /// a member of the named role.
        /// </returns>
        public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));

            if (string.IsNullOrEmpty(roleName))
            {
                return false;
            }

            var role = await _roleStore.FindByNameAsync(roleName, cancellationToken);
            if (role == null)
            {
                return false;
            }

            var userRoleQuery = GetUserRoleQuery(_unitOfWork.GetRepository<UserRole>(), user.Id, role.Id);
            var isInRole = await _queryMaterializer.Any(userRoleQuery, cancellationToken);
            return isInRole;
        }

        /// <summary>
        /// Remove the specified <paramref name="user" /> from the named role.
        /// </summary>
        /// <param name="user">The user to remove the named role from.</param>
        /// <param name="roleName">The name of the role to remove.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
        /// </returns>
        public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));

            if (string.IsNullOrEmpty(roleName))
            {
                return;
            }

            var role = await _roleStore.FindByNameAsync(roleName, cancellationToken);
            if (role == null)
            {
                return;
            }

            var repository = _unitOfWork.GetRepository<UserRole>();
            var userRoleQuery = GetUserRoleQuery(repository, user.Id, role.Id);
            var userRoles = await _queryMaterializer.ToList(userRoleQuery, cancellationToken);
            repository.Remove(userRoles);
            await _unitOfWork.SaveChanges(cancellationToken);
        }

        /// <summary>
        /// Gets the user role query.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>The user role query.</returns>
        private static IQueryable<UserRole> GetUserRoleQuery(IRepository<UserRole> repository, Guid userId, Guid roleId)
        {
            var userRoleQuery = repository.Query.Where(ur => ur.UserId == userId && ur.RoleId == roleId);
            return userRoleQuery;
        }

        #endregion Methods
    }
}