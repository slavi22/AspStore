using AspStore.Models.Account;

namespace AspStore.ViewModels;

public class CheckoutViewModel
{
    public List<AddressModel> UserAddress { get; set; }
    public HashSet<CartViewModel> Cart { get; set; }
}