using System.Collections.Generic;

namespace SampleStore.Services.Identity.Constants;

public static class Roles
{
    public const string Admin = "Admin";
    public const string Client = "Client";
    public const string SuperAdmin = "SuperAdmin";

    public static IEnumerable<string> All => [SuperAdmin, Admin, Client];
}