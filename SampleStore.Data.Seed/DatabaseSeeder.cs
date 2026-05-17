using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using SampleStore.Common.Extensions;
using SampleStore.Data.Entities.Identity;
using SampleStore.Data.Extensions;
using SampleStore.Data.Seed.Commands;

namespace SampleStore.Data.Seed;

public class DatabaseSeeder
{
    private readonly IAddUserToRolesCommandFactory _addUserToRolesCommandFactory;

    private readonly ICreateRolesCommandFactory _createRolesCommandFactory;

    private readonly ICreateSuperAdminCommandFactory _createSuperAdminCommandFactory;

    private readonly IQueryMaterializer _queryMaterializer;

    private readonly UserManager<User> _userManager;

    public DatabaseSeeder(
        UserManager<User> userManager,
        IQueryMaterializer queryMaterializer,
        ICreateSuperAdminCommandFactory createSuperAdminCommandFactory,
        ICreateRolesCommandFactory createRolesCommandFactory,
        IAddUserToRolesCommandFactory addUserToRolesCommandFactory)
    {
        _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
        _queryMaterializer = queryMaterializer.ThrowIfArgumentIsNull(nameof(queryMaterializer));
        _createSuperAdminCommandFactory = createSuperAdminCommandFactory.ThrowIfArgumentIsNull(nameof(createSuperAdminCommandFactory));
        _createRolesCommandFactory = createRolesCommandFactory.ThrowIfArgumentIsNull(nameof(createRolesCommandFactory));
        _addUserToRolesCommandFactory = addUserToRolesCommandFactory.ThrowIfArgumentIsNull(nameof(addUserToRolesCommandFactory));
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
        await _addUserToRolesCommandFactory.CreateCommand(superAdmin, roles).Do();
    }
}