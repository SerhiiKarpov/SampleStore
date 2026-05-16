using System.ComponentModel.DataAnnotations;

namespace SampleStore.UI.ViewModels.Identity;

public class DeletePersonalDataViewModel
{
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}