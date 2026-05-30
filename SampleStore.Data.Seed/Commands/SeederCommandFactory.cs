using System;
using System.Collections.Generic;

using SampleStore.Common.Commands;
using SampleStore.Common.Services;
using SampleStore.Data.Entities.Identity;
using SampleStore.Services.Identity;

namespace SampleStore.Data.Seed.Commands;

public class SeederCommandFactory(
    IUserManager userManager,
    IRoleManager roleManager,
    IQueryMaterializer queryMaterializer,
    IDateTime dateTimeService)
    : IAddUserToRolesCommandFactory, ICreateRolesCommandFactory, ICreateSuperAdminCommandFactory
{
    private readonly IDateTime _dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
    private readonly IQueryMaterializer _queryMaterializer = queryMaterializer ?? throw new ArgumentNullException(nameof(queryMaterializer));
    private readonly IRoleManager _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
    private readonly IUserManager _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

    ICommand<bool> IAddUserToRolesCommandFactory.CreateCommand(User user, IEnumerable<string> roleNames) =>
        new AddUserToRolesCommand(_userManager, user, roleNames);

    ICommand<List<Role>> ICreateRolesCommandFactory.CreateCommand() =>
        new CreateRolesCommand(_roleManager, _queryMaterializer);

    ICommand<User> ICreateSuperAdminCommandFactory.CreateCommand(User prototype, string password) =>
        new CreateSuperAdminCommand(_userManager, _dateTimeService, prototype, password);
}