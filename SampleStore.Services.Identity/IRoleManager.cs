using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity;

public interface IRoleManager
{
    IQueryable<Role> Roles { get; }
    Task<IdentityResult> CreateAsync(Role role);
}