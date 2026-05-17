using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using SampleStore.Common.Extensions;
using SampleStore.Data.Entities.Identity;
using SampleStore.Data.Extensions;

namespace SampleStore.Services.Identity;

public partial class UserStore : IUserAuthenticationTokenStore<User>
{
    public async Task<string?> GetTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);
        var token = await _unitOfWork.GetRepository<UserToken>().Find(
            GetUserTokenPredicate(user.Id, loginProvider, name),
            _queryMaterializer,
            cancellationToken);
        return token?.Value;
    }

    public async Task RemoveTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);

        var repository = _unitOfWork.GetRepository<UserToken>();

        var token = await repository.Find(GetUserTokenPredicate(user.Id, loginProvider, name), _queryMaterializer, cancellationToken);
        if (token == null)
        {
            return;
        }

        repository.Remove(token);
        await _unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task SetTokenAsync(User user, string loginProvider, string name, string? value, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);

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

    private static Expression<Func<UserToken, bool>> GetUserTokenPredicate(Guid userId, string loginProvider, string name)
    {
        return ut => ut.UserId == userId
            && ut.LoginProvider == loginProvider
            && ut.Name == name;
    }
}