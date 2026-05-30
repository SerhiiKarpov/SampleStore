using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity;

internal partial class UserStore : IUserTwoFactorRecoveryCodeStore<User>
{
    private const string TwoFactoryRecoveryCodeDelimiter = ";";

    private const string TwoFactoryRecoveryCodeTokenLoginProvider = "TwoFactoryRecoveryCodeTokenLoginProvider";

    private const string TwoFactoryRecoveryCodeTokenName = "TwoFactoryRecoveryCodeTokenName";

    public async Task<int> CountCodesAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);
        var codes = await GetTwoFactoryRecoveryCodes(user, cancellationToken);
        return codes.Count();
    }

    public async Task<bool> RedeemCodeAsync(User user, string code, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(code);

        var codes = await GetTwoFactoryRecoveryCodes(user, cancellationToken);
        if (!codes.Contains(code))
        {
            return false;
        }

        var updatedCodes = codes.Where(x => x != code).ToList();
        await ReplaceCodesAsync(user, updatedCodes, cancellationToken);
        return true;
    }

    public Task ReplaceCodesAsync(User user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
    {
        var token = string.Join(TwoFactoryRecoveryCodeDelimiter, recoveryCodes);
        return SetTokenAsync(user, TwoFactoryRecoveryCodeTokenLoginProvider, TwoFactoryRecoveryCodeTokenName, token, cancellationToken);
    }

    private async Task<IEnumerable<string>> GetTwoFactoryRecoveryCodes(User user, CancellationToken cancellationToken)
    {
        var token = await GetTokenAsync(user, TwoFactoryRecoveryCodeTokenLoginProvider, TwoFactoryRecoveryCodeTokenName, cancellationToken);
        var codes = (token ?? string.Empty).Split(TwoFactoryRecoveryCodeDelimiter);
        return codes;
    }
}