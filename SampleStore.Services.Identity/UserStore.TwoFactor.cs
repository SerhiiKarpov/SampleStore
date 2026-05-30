using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity;

internal partial class UserStore : IUserTwoFactorStore<User>
{
    public Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.TwoFactorEnabled);
    }

    public Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);
        user.TwoFactorEnabled = enabled;
        return Task.CompletedTask;
    }
}