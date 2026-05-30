using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SampleStore.Common.Commands;
using SampleStore.Data.Entities.Identity;
using SampleStore.Data.Seed.Extensions;
using SampleStore.Services.Identity;

namespace SampleStore.Data.Seed.Commands;

public class AddUserToRolesCommand : ICommand<bool>
{
    private readonly IEnumerable<string> _roleNames;
    private readonly User _user;
    private readonly IUserManager _userManager;

    public AddUserToRolesCommand(IUserManager userManager, User user, IEnumerable<string> roleNames)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _user = user ?? throw new ArgumentNullException(nameof(user));
        _roleNames = roleNames ?? throw new ArgumentNullException(nameof(roleNames));
    }

    public async Task<bool> Do()
    {
        var addToRolesResult = await _userManager.AddToRolesAsync(_user, _roleNames);
        addToRolesResult.ThrowIfFailed(() => $"Failed to add user {_user.Email} to roles: {string.Join(", ", _roleNames)}. Check logs for details.");
        return true;
    }
}