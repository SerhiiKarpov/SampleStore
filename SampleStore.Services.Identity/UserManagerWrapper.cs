using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity;

internal sealed class UserManagerWrapper(UserManager<User> wrappee) : IUserManager
{
    private readonly UserManager<User> _wrappee = wrappee ?? throw new ArgumentNullException(nameof(wrappee));

    public IQueryable<User> Users => _wrappee.Users;

    public IdentityOptions Options => _wrappee.Options;

    public async Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles) =>
        await _wrappee.AddToRolesAsync(user, roles);
    
    public async Task<IdentityResult> CreateAsync(User user, string password) =>
        await _wrappee.CreateAsync(user, password);

    public async Task<IdentityResult> CreateAsync(User user) =>
        await _wrappee.CreateAsync(user);
    
    public async Task<User?> FindByIdAsync(string userId) =>
        await _wrappee.FindByIdAsync(userId);

    public async Task<IdentityResult> ConfirmEmailAsync(User user, string token) =>
        await _wrappee.ConfirmEmailAsync(user, token);
    
    public async Task<User?> FindByEmailAsync(string email) =>
        await _wrappee.FindByEmailAsync(email);

    public async Task<IdentityResult> AddLoginAsync(User user, UserLoginInfo login) =>
        await _wrappee.AddLoginAsync(user, login);
    
    public async Task<IdentityResult> AddClaimsAsync(User user, IEnumerable<Claim> claims) =>
        await _wrappee.AddClaimsAsync(user, claims);

    public async Task<string> GenerateEmailConfirmationTokenAsync(User user) =>
        await _wrappee.GenerateEmailConfirmationTokenAsync(user);

    public async Task<bool> IsEmailConfirmedAsync(User user) =>
        await _wrappee.IsEmailConfirmedAsync(user);

    public async Task<string> GeneratePasswordResetTokenAsync(User user) =>
        await _wrappee.GeneratePasswordResetTokenAsync(user);

    public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword) =>
        await _wrappee.ResetPasswordAsync(user, token, newPassword);
    
    public async Task<User?> GetUserAsync(ClaimsPrincipal principal) =>
        await _wrappee.GetUserAsync(principal);
    
    public string? GetUserId(ClaimsPrincipal principal) =>
        _wrappee.GetUserId(principal);

    public async Task<bool> HasPasswordAsync(User user) =>
        await _wrappee.HasPasswordAsync(user);
    
    public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword) =>
        await _wrappee.ChangePasswordAsync(user, currentPassword, newPassword);

    public async Task<bool> CheckPasswordAsync(User user, string password) =>
        await _wrappee.CheckPasswordAsync(user, password);

    public async Task<IdentityResult> DeleteAsync(User user) =>
        await _wrappee.DeleteAsync(user);

    public async Task<bool> GetTwoFactorEnabledAsync(User user) =>
        await _wrappee.GetTwoFactorEnabledAsync(user);

    public async Task<IdentityResult> SetTwoFactorEnabledAsync(User user, bool enabled) =>
        await _wrappee.SetTwoFactorEnabledAsync(user, enabled);

    public async Task<bool> VerifyTwoFactorTokenAsync(User user, string tokenProvider, string token) =>
        await _wrappee.VerifyTwoFactorTokenAsync(user, tokenProvider, token);

    public async Task<int> CountRecoveryCodesAsync(User user) =>
        await _wrappee.CountRecoveryCodesAsync(user);

    public async Task<IEnumerable<string>?> GenerateNewTwoFactorRecoveryCodesAsync(User user, int number) =>
        await _wrappee.GenerateNewTwoFactorRecoveryCodesAsync(user, number);

    public async Task<string?> GetAuthenticatorKeyAsync(User user) =>
        await _wrappee.GetAuthenticatorKeyAsync(user);

    public async Task<IdentityResult> ResetAuthenticatorKeyAsync(User user) =>
        await _wrappee.ResetAuthenticatorKeyAsync(user);

    public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user) =>
        await _wrappee.GetLoginsAsync(user);

    public async Task<IdentityResult> RemoveLoginAsync(User user, string loginProvider, string providerKey) =>
        await _wrappee.RemoveLoginAsync(user, loginProvider, providerKey);

    public async Task<IdentityResult> SetEmailAsync(User user, string? email) =>
        await _wrappee.SetEmailAsync(user, email);

    public async Task<IdentityResult> SetPhoneNumberAsync(User user, string? phoneNumber) =>
        await _wrappee.SetPhoneNumberAsync(user, phoneNumber);

    public async Task<IdentityResult> UpdateAsync(User user) =>
        await _wrappee.UpdateAsync(user);

    public async Task<IdentityResult> AddPasswordAsync(User user, string password) =>
        await _wrappee.AddPasswordAsync(user, password);
}