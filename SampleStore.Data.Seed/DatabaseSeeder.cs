using System;
using System.Linq;
using System.Threading.Tasks;

using SampleStore.Data.Entities.Identity;
using SampleStore.Data.Extensions;
using SampleStore.Data.Seed.Commands;
using SampleStore.Services.Identity;

namespace SampleStore.Data.Seed;

public class DatabaseSeeder
{
    private readonly IAddUserToRolesCommandFactory _addUserToRolesCommandFactory;
    private readonly ICreateRolesCommandFactory _createRolesCommandFactory;
    private readonly ICreateSuperAdminCommandFactory _createSuperAdminCommandFactory;
    private readonly IQueryMaterializer _queryMaterializer;
    private readonly IUserManager _userManager;

    public DatabaseSeeder(
        IUserManager userManager,
        IQueryMaterializer queryMaterializer,
        ICreateSuperAdminCommandFactory createSuperAdminCommandFactory,
        ICreateRolesCommandFactory createRolesCommandFactory,
        IAddUserToRolesCommandFactory addUserToRolesCommandFactory)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _queryMaterializer = queryMaterializer ?? throw new ArgumentNullException(nameof(queryMaterializer));
        _createSuperAdminCommandFactory = createSuperAdminCommandFactory ?? throw new ArgumentNullException(nameof(createSuperAdminCommandFactory));
        _createRolesCommandFactory = createRolesCommandFactory ?? throw new ArgumentNullException(nameof(createRolesCommandFactory));
        _addUserToRolesCommandFactory = addUserToRolesCommandFactory ?? throw new ArgumentNullException(nameof(addUserToRolesCommandFactory));
    }

    public async Task<bool> NeedsSeeding()
    {
        var hasUsers = await _queryMaterializer.Any(_userManager.Users);
        return !hasUsers;
    }

    public async Task Seed(User superAdminPrototype, string defaultPassword)
    {
        var roles = await _createRolesCommandFactory.CreateCommand().Do();
        var superAdmin = await _createSuperAdminCommandFactory.CreateCommand(superAdminPrototype, defaultPassword).Do();
        await _addUserToRolesCommandFactory.CreateCommand(superAdmin, roles.Select(x => x.Name)).Do();
    }
}