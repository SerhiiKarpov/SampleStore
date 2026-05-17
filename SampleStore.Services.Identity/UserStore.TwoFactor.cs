using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using SampleStore.Common.Extensions;
using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity;

public partial class UserStore : IUserTwoFactorStore<User>
{
    public Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfArgumentIsNull(nameof(user));
        return Task.FromResult(user.TwoFactorEnabled);
    }

    public Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfArgumentIsNull(nameof(user));
        user.TwoFactorEnabled = enabled;
        return Task.CompletedTask;
    }
}