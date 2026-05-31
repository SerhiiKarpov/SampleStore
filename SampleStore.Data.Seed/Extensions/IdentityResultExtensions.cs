using System;
using System.Text;

using Microsoft.AspNetCore.Identity;

namespace SampleStore.Data.Seed.Extensions;

public static class IdentityResultExtensions
{
    public static void ThrowIfFailed(this IdentityResult result, Func<string> getMessage)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(getMessage);

        var errorMessage = result.GetErrorMessage(getMessage);
        if (errorMessage is not null)
        {
            throw new InvalidOperationException(errorMessage);
        }
    }

    public static string? GetErrorMessage(this IdentityResult result, Func<string> getMessage)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(getMessage);

        if (result.Succeeded)
        {
            return null;
        }

        var messageBuilder = new StringBuilder();
        messageBuilder.AppendLine(getMessage());
        messageBuilder.AppendLine("Identity Result Errors:");
        var errorCounter = 0;
        foreach (var error in result.Errors)
        {
            messageBuilder.AppendLine($"{++errorCounter}) Code: {error.Code}, Description: {error.Description}.");
        }

        return messageBuilder.ToString();
    }
}