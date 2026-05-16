using System.ComponentModel.DataAnnotations;

namespace SampleStore.UI.ViewModels.Identity;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}