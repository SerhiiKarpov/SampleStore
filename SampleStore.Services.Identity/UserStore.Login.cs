using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using SampleStore.Common.Extensions;
using SampleStore.Data.Entities.Identity;
using SampleStore.Data.Extensions;
using SampleStore.Services.Identity.Mapping;

namespace SampleStore.Services.Identity;

internal partial class UserStore : IUserLoginStore<User>
{
    public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(login);

        var userLogin = login.ToUserLogin();
        userLogin.UserId = user.Id;
        _unitOfWork.GetRepository<UserLogin>().Add(userLogin);
        await _unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task<User?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
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

    public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);

        var query = _unitOfWork.GetRepository<UserLogin>().Query.Where(ul => ul.UserId == user.Id);
        var userLogins = await _queryMaterializer.ToList(query, cancellationToken);
        var logins = userLogins.Select(x => x.ToUserLoginInfo()).ToList();
        return logins;
    }

    public async Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);

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
}