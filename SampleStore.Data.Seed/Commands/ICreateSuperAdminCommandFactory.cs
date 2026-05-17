using SampleStore.Common.Commands;
using SampleStore.Data.Entities.Identity;

namespace SampleStore.Data.Seed.Commands;

public interface ICreateSuperAdminCommandFactory
{
    ICommand<User> CreateCommand(User prototype, string password);
}