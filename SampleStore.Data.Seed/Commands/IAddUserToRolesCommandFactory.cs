using System.Collections.Generic;

using SampleStore.Common.Commands;
using SampleStore.Data.Entities.Identity;

namespace SampleStore.Data.Seed.Commands;

public interface IAddUserToRolesCommandFactory
{
    ICommand<bool> CreateCommand(User user, IEnumerable<Role> roles);
}