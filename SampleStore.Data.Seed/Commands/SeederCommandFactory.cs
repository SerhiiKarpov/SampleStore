using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

using SampleStore.Common.Commands;
using SampleStore.Common.Extensions;
using SampleStore.Common.Services;
using SampleStore.Data.Entities.Identity;

namespace SampleStore.Data.Seed.Commands;

public class SeederCommandFactory : IAddUserToRolesCommandFactory, ICreateRolesCommandFactory, ICreateSuperAdminCommandFactory
{
    private readonly IDateTime _dateTimeService;

    private readonly IQueryMaterializer _queryMaterializer;

    private readonly RoleManager<Role> _roleManager;

    private readonly UserManager<User> _userManager;

    public SeederCommandFactory(UserManager<User> userManager, RoleManager<Role> roleManager, IQueryMaterializer queryMaterializer, IDateTime dateTimeService)
    {
        _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
        _roleManager = roleManager.ThrowIfArgumentIsNull(nameof(roleManager));
        _queryMaterializer = queryMaterializer.ThrowIfArgumentIsNull(nameof(queryMaterializer));
        _dateTimeService = dateTimeService.ThrowIfArgumentIsNull(nameof(dateTimeService));
    }

    ICommand<bool> IAddUserToRolesCommandFactory.CreateCommand(User user, IEnumerable<Role> roles)
    {
        return new AddUserToRolesCommand(_userManager, _roleManager, user, roles);
    }

    ICommand<List<Role>> ICreateRolesCommandFactory.CreateCommand()
    {
        return new CreateRolesCommand(_roleManager, _queryMaterializer);
    }

    ICommand<User> ICreateSuperAdminCommandFactory.CreateCommand(User prototype, string password)
    {
        return new CreateSuperAdminCommand(_userManager, _dateTimeService, prototype, password);
    }
}