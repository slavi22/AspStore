using AspStore.Models.Product;

namespace AspStore.ViewModels;

public class OrderHistoryDetailsViewModel
{
    public string DeliveryAddress { get; set; }
    public string Recipient { get; set; }
    public string PhoneNumber { get; set; }
    public string OrderDate { get; set; }
    public List<ProductModel> Products { get; set; }
    public List<int> ProductQuantities { get; set; }
    
    public List<decimal?> ProductPrices { get; set; }
}