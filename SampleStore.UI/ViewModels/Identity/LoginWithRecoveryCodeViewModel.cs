using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace SampleStore.UI.ViewModels.Identity;

public class LoginWithRecoveryCodeViewModel
{
    [BindProperty]
    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Recovery Code")]
    public string RecoveryCode { get; set; } = string.Empty;
}