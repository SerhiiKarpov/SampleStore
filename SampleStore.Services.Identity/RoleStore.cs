using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using SampleStore.Common;
using SampleStore.Common.Extensions;
using SampleStore.Data;
using SampleStore.Data.Entities.Identity;
using SampleStore.Data.Extensions;

namespace SampleStore.Services.Identity;

public partial class RoleStore : Disposable, IRoleStore<Role>
{
    private readonly IQueryMaterializer _queryMaterializer;

    private readonly IUnitOfWork _unitOfWork;

    public RoleStore(IUnitOfWork unitOfWork, IQueryMaterializer queryMaterializer)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _queryMaterializer = queryMaterializer ?? throw new ArgumentNullException(nameof(queryMaterializer));
    }

    public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(role);

        _unitOfWork.GetRepository<Role>().Add(role);
        await _unitOfWork.SaveChanges(cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(role);

        _unitOfWork.GetRepository<Role>().Remove(role);
        await _unitOfWork.SaveChanges(cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<Role?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!Guid.TryParse(roleId, out var id))
        {
            return null;
        }

        var role = await _unitOfWork.GetRepository<Role>().FindById(id, _queryMaterializer, cancellationToken);
        return role;
    }

    public Task<Role?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        return _unitOfWork.GetRepository<Role>().Find(r => r.Name == normalizedRoleName, _queryMaterializer, cancellationToken);
    }

    public Task<string?> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(role);
        return Task.FromResult<string?>(role.Name);
    }

    public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(role);
        return Task.FromResult(role.Id.ToString());
    }

    public Task<string?> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(role);
        return Task.FromResult<string?>(role.Name);
    }

    public Task SetNormalizedRoleNameAsync(Role role, string? normalizedName, CancellationToken cancellationToken)
    {
        return SetRoleNameAsync(role, normalizedName, cancellationToken);
    }

    public Task SetRoleNameAsync(Role role, string? roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(role);
        role.Name = roleName!;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(role);

        await _unitOfWork.Update(role, _queryMaterializer, cancellationToken);
        return IdentityResult.Success;
    }
}