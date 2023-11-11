using System.ComponentModel.DataAnnotations;

namespace AspStore.Models;

public class RegisterModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage ="Password doesn't match!")]
    public string ConfirmPassword { get; set; }
}