using System.Linq;

using Microsoft.AspNetCore.Identity;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity;

internal partial class RoleStore : IQueryableRoleStore<Role>
{
    public IQueryable<Role> Roles => _unitOfWork.GetRepository<Role>().Query;
}