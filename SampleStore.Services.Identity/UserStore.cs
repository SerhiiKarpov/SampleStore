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

public partial class UserStore : Disposable, IUserStore<User>
{
    private readonly IQueryMaterializer _queryMaterializer;

    private readonly IRoleStore<Role> _roleStore;

    private readonly IUnitOfWork _unitOfWork;

    public UserStore(
        IUnitOfWork unitOfWork,
        IQueryMaterializer queryMaterializer,
        IRoleStore<Role> roleStore)
    {
        _unitOfWork = unitOfWork.ThrowIfArgumentIsNull(nameof(unitOfWork));
        _queryMaterializer = queryMaterializer.ThrowIfArgumentIsNull(nameof(queryMaterializer));
        _roleStore = roleStore.ThrowIfArgumentIsNull(nameof(roleStore));
    }

    public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfArgumentIsNull(nameof(user));

        _unitOfWork.GetRepository<User>().Add(user);
        await _unitOfWork.SaveChanges(cancellationToken);

        return IdentityResult.Success;
    }

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

    public async Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!Guid.TryParse(userId, out var id))
        {
            return null;
        }

        var user = await _unitOfWork.GetRepository<User>().FindById(id, _queryMaterializer, cancellationToken);
        return user;
    }

    public async Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await _unitOfWork.GetRepository<User>().Find(u => u.Email == normalizedUserName, _queryMaterializer, cancellationToken);
        return user;
    }

    public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return GetUserNameAsync(user, cancellationToken);
    }

    public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfArgumentIsNull(nameof(user));
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string?> GetUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return GetEmailAsync(user, cancellationToken);
    }

    public Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
    {
        return SetUserNameAsync(user, normalizedName, cancellationToken);
    }

    public Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
    {
        return SetEmailAsync(user, userName, cancellationToken);
    }

    public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfArgumentIsNull(nameof(user));

        await _unitOfWork.GetRepository<User>().Update(user, _queryMaterializer, cancellationToken);
        return IdentityResult.Success;
    }
}