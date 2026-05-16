using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

using Microsoft.AspNetCore.Identity;

using SampleStore.Data.Entities.Identity;
using SampleStore.UI.ViewModels.Identity;

namespace SampleStore.UI.Mapping;

internal static class UiMapper
{
    public static void Map(ExternalLoginViewModel? source, User? destination)
    {
        if (source is null || destination is null)
        {
            return;
        }

        destination.FullName = source.Name;
        destination.DateOfBirth = source.DateOfBirth ?? default;
        destination.Email = source.Email;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static User? ToUser(this ExternalLoginViewModel? source)
    {
        if (source is null)
        {
            return null;
        }

        var destination = new User();
        Map(source, destination);
        return destination;
    }

    public static void Map(RegistrationViewModel? source, User? destination)
    {
        if (source is null || destination is null)
        {
            return;
        }

        destination.FullName = source.Name;
        destination.Email = source.Email;
        destination.DateOfBirth = source.DateOfBirth;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static User? ToUser(this RegistrationViewModel? source)
    {
        if (source is null)
        {
            return null;
        }

        var destination = new User();
        Map(source, destination);
        return destination;
    }

    public static void Map(ExternalLoginInfo? source, ExternalLoginViewModel? destination)
    {
        if (source is null || destination is null)
        {
            return;
        }

        destination.Email = source.Principal.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        destination.Name =
            source.Principal.HasClaim(claim => claim.Type == ClaimTypes.GivenName)
                    && source.Principal.HasClaim(claim => claim.Type == ClaimTypes.Surname)
                ? $"{source.Principal.FindFirstValue(ClaimTypes.GivenName)} {source.Principal.FindFirstValue(ClaimTypes.Surname)}"
                : source.Principal.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
        destination.DateOfBirth =
            source.Principal.FindFirstValue(ClaimTypes.DateOfBirth) is { } dobString
                    && DateTime.TryParse(dobString, out var dob)
                ? dob
                : null;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static ExternalLoginViewModel? ToExternalLoginViewModel(this ExternalLoginInfo? source)
    {
        if (source is null)
        {
            return null;
        }

        var destination = new ExternalLoginViewModel();
        Map(source, destination);
        return destination;
    }
}