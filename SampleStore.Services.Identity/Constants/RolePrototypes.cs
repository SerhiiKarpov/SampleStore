using System.Collections.Generic;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity.Constants;

public static class RolePrototypes
{
    public static readonly Role Admin = new Role
    {
        Name = "Admin"
    };

    public static readonly Role Client = new Role
    {
        Name = "Client"
    };

    public static readonly Role SuperAdmin = new Role
    {
        Name = "SuperAdmin"
    };

    public static IEnumerable<Role> Roles
    {
        get
        {
            yield return SuperAdmin;
            yield return Admin;
            yield return Client;
        }
    }
}