using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity;

internal sealed class SignInManagerWrapper(SignInManager<User> wrappee) : ISignInManager
{
    private readonly SignInManager<User> _wrappee = wrappee;

    public async Task RefreshSignInAsync(User user) =>
        await _wrappee.RefreshSignInAsync(user);

    public async Task SignOutAsync() =>
        await _wrappee.SignOutAsync();

    public async Task ForgetTwoFactorClientAsync() =>
        await _wrappee.ForgetTwoFactorClientAsync();

    public async Task<bool> IsTwoFactorClientRememberedAsync(User user) =>
        await _wrappee.IsTwoFactorClientRememberedAsync(user);

    public async Task<ExternalLoginInfo?> GetExternalLoginInfoAsync(string? expectedXsrf = null) =>
        await _wrappee.GetExternalLoginInfoAsync(expectedXsrf);

    public async Task<SignInResult> ExternalLoginSignInAsync(
        string loginProvider,
        string providerKey,
        bool isPersistent,
        bool bypassTwoFactor) =>
        await _wrappee.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);

    public AuthenticationProperties ConfigureExternalAuthenticationProperties(
        string? provider,
         string? redirectUrl,
          string? userId = null) =>
        _wrappee.ConfigureExternalAuthenticationProperties(provider, redirectUrl, userId);

    public async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync() =>
        await _wrappee.GetExternalAuthenticationSchemesAsync();
    
    public async Task<SignInResult> PasswordSignInAsync(
        string userName,
        string password,
        bool isPersistent,
        bool lockoutOnFailure) =>
        await _wrappee.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);

    public async Task<User?> GetTwoFactorAuthenticationUserAsync() =>
        await _wrappee.GetTwoFactorAuthenticationUserAsync();

    public async Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string code, bool isPersistent, bool rememberClient) =>
        await _wrappee.TwoFactorAuthenticatorSignInAsync(code, isPersistent, rememberClient);

    public async Task<SignInResult> TwoFactorRecoveryCodeSignInAsync(string recoveryCode) =>
        await _wrappee.TwoFactorRecoveryCodeSignInAsync(recoveryCode);
}