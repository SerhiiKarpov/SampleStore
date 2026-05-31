using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using SampleStore.Common.Extensions;
using SampleStore.Data.Entities.Identity;
using SampleStore.Data.Seed.Configuration;
using SampleStore.Data.Seed.Extensions;
using SampleStore.Services.Identity;

namespace SampleStore.Data.Seed.Commands;

internal sealed class CreateSuperAdminCommand(
    IUserManager userManager,
    IOptions<SuperAdminOptions> options) : IDatabaseSeederCommand
{
    internal static IEnumerable<string> Roles { get; } = [Services.Identity.Constants.Roles.SuperAdmin];

    private readonly IOptions<SuperAdminOptions> _options = options ?? throw new ArgumentNullException(nameof(options));
    private readonly IUserManager _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

    public async Task Do()
    {
        var options = _options.Value;
        User superAdmin = CreateUser(options);

        var userResult = await _userManager.CreateAsync(superAdmin, options.Password);
        userResult.ThrowIfFailed(() => GetCreateUserErrorMessage(superAdmin.Email));

        var rolesResult = await _userManager.AddToRolesAsync(superAdmin, Roles);
        rolesResult.ThrowIfFailed(() => GetAddToRolesErrorMessage(superAdmin.Email, Roles));
    }

    internal static string GetCreateUserErrorMessage(string email) =>
        $"Failed to create user {email}";

    internal static string GetAddToRolesErrorMessage(string email, IEnumerable<string> roles) =>
        $"Failed to add user {email} to roles: {string.Join(",", roles)}";

    private static User CreateUser(SuperAdminOptions options)
    {
        var superAdmin = new User();
        options.Prototype.CopyTo(superAdmin);
        superAdmin.DateOfBirth = DateTime.UtcNow.Date;
        superAdmin.EmailConfirmed = true;
        return superAdmin;
    }
}