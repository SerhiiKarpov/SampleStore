using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity;

internal sealed class RoleManagerWrapper(RoleManager<Role> wrappee) : IRoleManager
{
    private readonly RoleManager<Role> _wrappee = wrappee ?? throw new ArgumentNullException(nameof(wrappee));

    public IQueryable<Role> Roles => _wrappee.Roles;

    public async Task<IdentityResult> CreateAsync(Role role) => await _wrappee.CreateAsync(role);
}