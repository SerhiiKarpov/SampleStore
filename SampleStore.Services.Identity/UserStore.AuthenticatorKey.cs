using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity;

internal partial class UserStore : IUserAuthenticatorKeyStore<User>
{
    private const string AuthenticatorKeyTokenLoginProvider = "AuthenticatorKeyTokenLoginProvider";

    private const string AuthenticatorKeyTokenName = "AuthenticatorKeyTokenName";

    public Task<string?> GetAuthenticatorKeyAsync(User user, CancellationToken cancellationToken)
    {
        return GetTokenAsync(user, AuthenticatorKeyTokenLoginProvider, AuthenticatorKeyTokenName, cancellationToken);
    }

    public Task SetAuthenticatorKeyAsync(User user, string key, CancellationToken cancellationToken)
    {
        return SetTokenAsync(user, AuthenticatorKeyTokenLoginProvider, AuthenticatorKeyTokenName, key, cancellationToken);
    }
}