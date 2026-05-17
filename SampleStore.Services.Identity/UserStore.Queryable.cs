using System.Linq;

using Microsoft.AspNetCore.Identity;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity;

public partial class UserStore : IQueryableUserStore<User>
{
    public IQueryable<User> Users
    {
        get
        {
            return _unitOfWork.GetRepository<User>().Query;
        }
    }
}