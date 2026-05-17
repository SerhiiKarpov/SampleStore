using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using SampleStore.Common.Extensions;
using SampleStore.Data;
using SampleStore.Data.Entities.Identity;
using SampleStore.Data.Extensions;

namespace SampleStore.Services.Identity;

public partial class UserStore : IUserRoleStore<User>
{
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
            roleNames.Add(roleName!);
        }

        return roleNames;
    }

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

    private static IQueryable<UserRole> GetUserRoleQuery(IRepository<UserRole> repository, Guid userId, Guid roleId)
    {
        var userRoleQuery = repository.Query.Where(ur => ur.UserId == userId && ur.RoleId == roleId);
        return userRoleQuery;
    }
}