using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

using Microsoft.AspNetCore.Identity;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Services.Identity.Mapping;

internal static class IdentityMapper
{
    public static void Map(UserLoginInfo? source, UserLogin? destination)
    {
        if (source is null || destination is null)
        {
            return;
        }

        destination.LoginProvider = source.LoginProvider;
        destination.ProviderDisplayName = source.ProviderDisplayName!;
        destination.ProviderKey = source.ProviderKey;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static UserLogin? ToUserLogin(this UserLoginInfo? source)
    {
        if (source is null)
        {
            return null;
        }

        var destination = new UserLogin
        {
            LoginProvider = source.LoginProvider,
            ProviderDisplayName = source.ProviderDisplayName!,
            ProviderKey = source.ProviderKey
        };
        return destination;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static UserLoginInfo? ToUserLoginInfo(this UserLogin? source)
    {
        if (source is null)
        {
            return null;
        }

        return new(source.LoginProvider, source.ProviderKey, source.ProviderDisplayName);
    }

    public static void Map(Claim? source, UserClaim? destination)
    {
        if (source is null || destination is null)
        {
            return;
        }

        destination.Type = source.Type;
        destination.Value = source.Value;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static UserClaim? ToUserClaim(this Claim? source)
    {
        if (source is null)
        {
            return null;
        }

        var destination = new UserClaim();
        Map(source, destination);
        return destination;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static Claim? ToClaim(this UserClaim? source)
    {
        if (source is null)
        {
            return null;
        }

        return new(source.Type!, source.Value!);
    }
}