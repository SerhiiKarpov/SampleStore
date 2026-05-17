using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using SampleStore.Data.Entities.Identity;
using SampleStore.Services.Identity.Mapping;

namespace SampleStore.Services.Identity;

public partial class UserStore : IUserClaimStore<User>
{
    public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);

        if (claims == null)
        {
            return;
        }

        var userClaims = claims.Select(x => x.ToUserClaim()).ToList();
        foreach (var userClaim in userClaims)
        {
            userClaim.UserId = user.Id;
        }

        _unitOfWork.GetRepository<UserClaim>().Add(userClaims);
        await _unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);

        var userClaims = await DoGetClaims(user, cancellationToken);
        var claims = userClaims.Select(x => x.ToClaim()).ToList();
        return claims;
    }

    public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(claim);

        var userClaims = _unitOfWork.GetRepository<UserClaim>().Query;
        var query =
            from user in _unitOfWork.GetRepository<User>().Query
            where userClaims.Any(uc => uc.UserId == user.Id && uc.Type == claim.Type && uc.Value == claim.Value)
            select user;
        var users = await _queryMaterializer.ToList(query, cancellationToken);
        return users;
    }

    public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);

        if (claims == null)
        {
            return;
        }

        var userClaims = await DoGetClaims(user, cancellationToken);
        var claimsToRemove =
            from uc in userClaims
            join c in claims on new { uc.Type, uc.Value } equals new { c.Type, c.Value }
            select uc;
        _unitOfWork.GetRepository<UserClaim>().Remove(claimsToRemove);
    }

    public async Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(claim);
        ArgumentNullException.ThrowIfNull(newClaim);

        var query =
            from uc in _unitOfWork.GetRepository<UserClaim>().Query
            where uc.UserId == user.Id
                && uc.Type == claim.Type
                && uc.Value == claim.Value
            select uc;
        var userClaims = await _queryMaterializer.ToList(query, cancellationToken);
        foreach (var userClaim in userClaims)
        {
            userClaim.Type = newClaim.Type;
            userClaim.Value = newClaim.Value;
        }
    }

    private async Task<List<UserClaim>> DoGetClaims(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);

        var query =
            from uc in _unitOfWork.GetRepository<UserClaim>().Query
            where uc.UserId == user.Id
            select uc;
        var userClaims = await _queryMaterializer.ToList(query, cancellationToken);
        return userClaims;
    }
}