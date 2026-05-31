using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SampleStore.Data.Extensions;
using SampleStore.Data.Seed.Commands;
using SampleStore.Services.Identity;

namespace SampleStore.Data.Seed;

internal sealed class DatabaseSeeder(
    IUserManager userManager,
    IQueryMaterializer queryMaterializer,
    IEnumerable<IDatabaseSeederCommand> commands) : IDatabaseSeeder
{
    private readonly IQueryMaterializer _queryMaterializer = queryMaterializer ?? throw new ArgumentNullException(nameof(queryMaterializer));
    private readonly IEnumerable<IDatabaseSeederCommand> _commands = commands ?? throw new ArgumentNullException(nameof(commands));
    private readonly IUserManager _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

    public async Task EnsureSeeded()
    {
        if (!await NeedsSeeding())
        {
            return;
        }

        foreach (var command in _commands)
        {
            await command.Do();
        }
    }

    private async Task<bool> NeedsSeeding()
    {
        var hasUsers = await _queryMaterializer.Any(_userManager.Users);
        return !hasUsers;
    }
}