using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AspStore.Models.Account;

public class AddressModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    [Required]
    [DisplayName("Short name")]
    public string ShortName { get; set; }
    [Required]
    [MaxLength(10)]
    [DisplayName("Phone number")]
    public string PhoneNumber { get; set; }
    [Required]
    public string Recipient { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string Address { get; set; }
    [ValidateNever]
    public string UserId { get; set; }
    [ValidateNever]
    public IdentityUser User { get; set; }
}