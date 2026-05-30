using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity;

public interface IUserManager
{
    IQueryable<User> Users { get; }
    IdentityOptions Options { get; }
    Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles);
    Task<IdentityResult> CreateAsync(User user, string password);
    Task<IdentityResult> CreateAsync(User user);
    Task<User?> FindByIdAsync(string userId);
    Task<IdentityResult> ConfirmEmailAsync(User user, string token);
    Task<bool> IsEmailConfirmedAsync(User user);
    Task<User?> FindByEmailAsync(string email);
    Task<IdentityResult> AddLoginAsync(User user, UserLoginInfo login);
    Task<IdentityResult> AddClaimsAsync(User user, IEnumerable<Claim> claims);
    Task<string> GenerateEmailConfirmationTokenAsync(User user);
    Task<string> GeneratePasswordResetTokenAsync(User user);
    Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);
    Task<User?> GetUserAsync(ClaimsPrincipal principal);
    string? GetUserId(ClaimsPrincipal principal);
    Task<bool> HasPasswordAsync(User user);
    Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);
    Task<bool> CheckPasswordAsync(User user, string password);
    Task<IdentityResult> DeleteAsync(User user);
    Task<bool> GetTwoFactorEnabledAsync(User user);
    Task<IdentityResult> SetTwoFactorEnabledAsync(User user, bool enabled);
    Task<bool> VerifyTwoFactorTokenAsync(User user, string tokenProvider, string token);
    Task<int> CountRecoveryCodesAsync(User user);
    Task<IEnumerable<string>?> GenerateNewTwoFactorRecoveryCodesAsync(User user, int number);
    Task<string?> GetAuthenticatorKeyAsync(User user);
    Task<IdentityResult> ResetAuthenticatorKeyAsync(User user);
    Task<IList<UserLoginInfo>> GetLoginsAsync(User user);
    Task<IdentityResult> RemoveLoginAsync(User user, string loginProvider, string providerKey);
    Task<IdentityResult> SetEmailAsync(User user, string? email);
    Task<IdentityResult> SetPhoneNumberAsync(User user, string? phoneNumber);
    Task<IdentityResult> UpdateAsync(User user);
    Task<IdentityResult> AddPasswordAsync(User user, string password);
}