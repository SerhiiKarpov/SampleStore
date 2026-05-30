using System.Linq;

using Microsoft.AspNetCore.Identity;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity;

internal partial class UserStore : IQueryableUserStore<User>
{
    public IQueryable<User> Users => _unitOfWork.GetRepository<User>().Query;
}