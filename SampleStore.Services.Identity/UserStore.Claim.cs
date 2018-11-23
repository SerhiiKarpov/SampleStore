namespace SampleStore.Services.Identity
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using SampleStore.Common.Extensions;
    using SampleStore.Data.Entities.Identity;
    using SampleStore.Mapping.Extensions;

    /// <summary>
    /// Class encapsulating user store.
    /// </summary>
    /// <seealso cref="IUserClaimStore{User}" />
    public partial class UserStore : IUserClaimStore<User>
    {
        #region Methods

        /// <summary>
        /// Add claims to a user as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to add the claim to.</param>
        /// <param name="claims">The collection of <see cref="T:System.Security.Claims.Claim" />s to add.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));

            if (claims == null)
            {
                return;
            }

            var userClaims = _mapperProvider.GetMapper<Claim, UserClaim>().Map(claims).ToList();
            foreach (var userClaim in userClaims)
            {
                userClaim.UserId = user.Id;
            }

            _unitOfWork.GetRepository<UserClaim>().Add(userClaims);
            await _unitOfWork.SaveChanges(cancellationToken);
        }

        /// <summary>
        /// Gets a list of <see cref="T:System.Security.Claims.Claim" />s to be belonging to the specified <paramref name="user" /> as an asynchronous operation.
        /// </summary>
        /// <param name="user">The role whose claims to retrieve.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the result of the asynchronous query, a list of <see cref="T:System.Security.Claims.Claim" />s.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));

            var userClaims = await DoGetClaims(user, cancellationToken);
            var claims = _mapperProvider.GetMapper<UserClaim, Claim>().Map(userClaims).ToList();
            return claims;
        }

        /// <summary>
        /// Returns a list of users who contain the specified <see cref="T:System.Security.Claims.Claim" />.
        /// </summary>
        /// <param name="claim">The claim to look for.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// A <see cref="Task{T}" /> that represents the result of the asynchronous query, a list of <see cref="User"/> who
        /// contain the specified claim.
        /// </returns>
        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            claim.ThrowIfArgumentIsNull(nameof(claim));

            var userClaims = _unitOfWork.GetRepository<UserClaim>().Query;
            var query =
                from user in _unitOfWork.GetRepository<User>().Query
                where userClaims.Any(uc => uc.UserId == user.Id && uc.Type == claim.Type && uc.Value == claim.Value)
                select user;
            var users = await _queryMaterializer.ToList(query, cancellationToken);
            return users;
        }

        /// <summary>
        /// Removes the specified <paramref name="claims" /> from the given <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user to remove the specified <paramref name="claims" /> from.</param>
        /// <param name="claims">A collection of <see cref="T:System.Security.Claims.Claim" />s to remove.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));

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

        /// <summary>
        /// Replaces the given <paramref name="claim" /> on the specified <paramref name="user" /> with the <paramref name="newClaim" />
        /// </summary>
        /// <param name="user">The user to replace the claim on.</param>
        /// <param name="claim">The claim to replace.</param>
        /// <param name="newClaim">The new claim to replace the existing <paramref name="claim" /> with.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public async Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));
            claim.ThrowIfArgumentIsNull(nameof(claim));
            newClaim.ThrowIfArgumentIsNull(nameof(newClaim));

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

        /// <summary>
        /// Does the get claims.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The get claims.
        /// </returns>
        private async Task<List<UserClaim>> DoGetClaims(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));

            var query =
                from uc in _unitOfWork.GetRepository<UserClaim>().Query
                where uc.UserId == user.Id
                select uc;
            var userClaims = await _queryMaterializer.ToList(query, cancellationToken);
            return userClaims;
        }

        #endregion Methods
    }
}