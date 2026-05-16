using System;
using System.ComponentModel.DataAnnotations;

namespace SampleStore.UI.ViewModels.Identity;

public class ExternalLoginViewModel
{
    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date Of Birth")]
    public DateTime? DateOfBirth { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;
}