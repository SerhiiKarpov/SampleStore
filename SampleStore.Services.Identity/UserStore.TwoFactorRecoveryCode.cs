namespace SampleStore.Services.Identity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using SampleStore.Common.Extensions;
    using SampleStore.Data.Entities.Identity;

    /// <summary>
    /// Class encapsulating user store.
    /// </summary>
    /// <seealso cref="IUserTwoFactorRecoveryCodeStore{User}" />
    public partial class UserStore : IUserTwoFactorRecoveryCodeStore<User>
    {
        #region Fields

        /// <summary>
        /// The two factory recovery code delimiter
        /// </summary>
        private const string TwoFactoryRecoveryCodeDelimiter = ";";

        /// <summary>
        /// The two factory recovery code token login provider
        /// </summary>
        private const string TwoFactoryRecoveryCodeTokenLoginProvider = "TwoFactoryRecoveryCodeTokenLoginProvider";

        /// <summary>
        /// The two factory recovery code token name
        /// </summary>
        private const string TwoFactoryRecoveryCodeTokenName = "TwoFactoryRecoveryCodeTokenName";

        #endregion Fields

        #region Methods

        /// <summary>
        /// Returns how many recovery code are still valid for a user.
        /// </summary>
        /// <param name="user">The user who owns the recovery code.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The number of valid recovery codes for the user..
        /// </returns>
        public async Task<int> CountCodesAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));
            var codes = await GetTwoFactoryRecoveryCodes(user, cancellationToken);
            return codes.Count();
        }

        /// <summary>
        /// Returns whether a recovery code is valid for a user. Note: recovery codes are only valid
        /// once, and will be invalid after use.
        /// </summary>
        /// <param name="user">The user who owns the recovery code.</param>
        /// <param name="code">The recovery code to use.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// True if the recovery code was found for the user.
        /// </returns>
        public async Task<bool> RedeemCodeAsync(User user, string code, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfArgumentIsNull(nameof(user));
            code.ThrowIfArgumentIsNull(nameof(code));

            var codes = await GetTwoFactoryRecoveryCodes(user, cancellationToken);
            if (!codes.Contains(code))
            {
                return false;
            }

            var updatedCodes = codes.Where(x => x != code).ToList();
            await ReplaceCodesAsync(user, updatedCodes, cancellationToken);
            return true;
        }

        /// <summary>
        /// Updates the recovery codes for the user while invalidating any previous recovery codes.
        /// </summary>
        /// <param name="user">The user to store new recovery codes for.</param>
        /// <param name="recoveryCodes">The new recovery codes for the user.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The new recovery codes for the user.
        /// </returns>
        public Task ReplaceCodesAsync(User user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
        {
            var token = string.Join(TwoFactoryRecoveryCodeDelimiter, recoveryCodes);
            return SetTokenAsync(user, TwoFactoryRecoveryCodeTokenLoginProvider, TwoFactoryRecoveryCodeTokenName, token, cancellationToken);
        }

        /// <summary>
        /// Gets the two factory recovery codes.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The two factory recovery codes.</returns>
        private async Task<IEnumerable<string>> GetTwoFactoryRecoveryCodes(User user, CancellationToken cancellationToken)
        {
            var token = await GetTokenAsync(user, TwoFactoryRecoveryCodeTokenLoginProvider, TwoFactoryRecoveryCodeTokenName, cancellationToken);
            var codes = (token ?? string.Empty).Split(TwoFactoryRecoveryCodeDelimiter);
            return codes;
        }

        #endregion Methods
    }
}