using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity;

public interface ISignInManager
{
    Task RefreshSignInAsync(User user);
    Task SignOutAsync();
    Task ForgetTwoFactorClientAsync();
    Task<bool> IsTwoFactorClientRememberedAsync(User user);
    Task<ExternalLoginInfo?> GetExternalLoginInfoAsync(string? expectedXsrf = null);
    Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor);
    AuthenticationProperties ConfigureExternalAuthenticationProperties(string? provider, string? redirectUrl, string? userId = null);
    Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();
    Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
    Task<User?> GetTwoFactorAuthenticationUserAsync();
    Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string code, bool isPersistent, bool rememberClient);
    Task<SignInResult> TwoFactorRecoveryCodeSignInAsync(string recoveryCode);
}