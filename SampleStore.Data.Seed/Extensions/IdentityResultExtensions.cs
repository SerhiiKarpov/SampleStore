using System;
using System.Text;

using Microsoft.AspNetCore.Identity;

using SampleStore.Common.Extensions;

namespace SampleStore.Data.Seed.Extensions;

public static class IdentityResultExtensions
{
    public static void ThrowIfFailed(this IdentityResult result, Func<string> getMessage)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(getMessage);

        if (result.Succeeded)
        {
            return;
        }

        var messageBuilder = new StringBuilder();
        messageBuilder.AppendLine(getMessage());
        messageBuilder.AppendLine("Identity Result Errors:");
        var errorCounter = 0;
        foreach (var error in result.Errors)
        {
            messageBuilder.Append($"{++errorCounter}) Code: {error.Code}, Description: {error.Description}.");
        }

        throw new InvalidOperationException(messageBuilder.ToString());
    }
}