using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using SampleStore.Common.Commands;
using SampleStore.Common.Extensions;
using SampleStore.Common.Services;
using SampleStore.Data.Entities.Identity;
using SampleStore.Data.Seed.Extensions;

namespace SampleStore.Data.Seed.Commands;

public class CreateSuperAdminCommand : ICommand<User>
{
    private readonly IDateTime _dateTimeService;

    private readonly string _password;

    private readonly User _prototype;

    private readonly UserManager<User> _userManager;

    public CreateSuperAdminCommand(UserManager<User> userManager, IDateTime dateTimeService, User prototype, string password)
    {
        _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
        _dateTimeService = dateTimeService.ThrowIfArgumentIsNull(nameof(dateTimeService));
        _prototype = prototype.ThrowIfArgumentIsNull(nameof(prototype));
        _password = password.ThrowIfArgumentIsNull(nameof(password));
    }

    public async Task<User> Do()
    {
        var superAdmin = new User { Email = string.Empty, FullName = string.Empty };
        _prototype.CopyTo(superAdmin);
        superAdmin.DateOfBirth = _dateTimeService.UtcNow.Date;
        superAdmin.EmailConfirmed = true;

        var userResult = await _userManager.CreateAsync(superAdmin, _password);
        userResult.ThrowIfFailed(() => $"Failed to create user {superAdmin.Email}");

        return superAdmin;
    }
}