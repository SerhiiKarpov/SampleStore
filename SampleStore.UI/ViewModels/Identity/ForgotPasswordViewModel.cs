
using System.ComponentModel.DataAnnotations;

namespace SampleStore.UI.ViewModels.Identity;
/// <summary>
/// Class encapsulating forgot password view model.
/// </summary>
public class ForgotPasswordViewModel
{
    #region Properties

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>
    /// The email.
    /// </value>
    [Required]
    [EmailAddress]
    public string Email
    {
        get; set;
    }

    #endregion Properties
}