using System;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SampleStore.UI.Extensions;

public static class ModelStateDictionaryExtensions
{
    public static ModelStateDictionary AddModelErrors(this ModelStateDictionary modelState, IdentityResult result)
    {
        ArgumentNullException.ThrowIfNull(modelState);
        ArgumentNullException.ThrowIfNull(result);

        foreach (var error in result.Errors)
        {
            modelState.AddModelError(string.Empty, error.Description);
        }

        return modelState;
    }
}