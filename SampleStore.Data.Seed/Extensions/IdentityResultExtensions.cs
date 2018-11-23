namespace SampleStore.Data.Seed.Extensions
{
    using System;
    using System.Text;

    using Microsoft.AspNetCore.Identity;
    using SampleStore.Common.Extensions;

    /// <summary>
    /// Class encapsulating identity result extensions.
    /// </summary>
    public static class IdentityResultExtensions
    {
        #region Methods

        /// <summary>
        /// Throws the error.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="getMessage">The message getter.</param>
        /// <exception cref="InvalidOperationException">The thrown error.</exception>
        public static void ThrowIfFailed(this IdentityResult result, Func<string> getMessage)
        {
            result.ThrowIfArgumentIsNull(nameof(result));
            getMessage.ThrowIfArgumentIsNull(nameof(getMessage));

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

        #endregion Methods
    }
}