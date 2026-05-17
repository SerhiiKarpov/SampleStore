using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using SampleStore.Common.Extensions;
using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity;

public partial class UserStore : IUserPhoneNumberStore<User>
{
    public Task<string?> GetPhoneNumberAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfArgumentIsNull(nameof(user));
        return Task.FromResult(user.PhoneNumber);
    }

    public Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfArgumentIsNull(nameof(user));
        return Task.FromResult(user.PhoneNumberConfirmed);
    }

    public Task SetPhoneNumberAsync(User user, string? phoneNumber, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfArgumentIsNull(nameof(user));
        user.PhoneNumber = phoneNumber;
        return Task.CompletedTask;
    }

    public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfArgumentIsNull(nameof(user));
        user.PhoneNumberConfirmed = confirmed;
        return Task.CompletedTask;
    }
}