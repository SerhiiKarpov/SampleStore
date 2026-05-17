using System.Linq;

using Microsoft.AspNetCore.Identity;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity;

public partial class RoleStore : IQueryableRoleStore<Role>
{
    public IQueryable<Role> Roles
    {
        get
        {
            return _unitOfWork.GetRepository<Role>().Query;
        }
    }
}