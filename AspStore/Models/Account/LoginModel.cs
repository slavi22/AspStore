using System.ComponentModel.DataAnnotations;

namespace AspStore.Models.Account;

public class LoginModel
{
    [Required] [EmailAddress] public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember Me")] public bool RememberMe { get; set; }
}