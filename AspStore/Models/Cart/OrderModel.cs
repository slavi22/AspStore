using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspStore.Models.Cart;

public class OrderModel
{
    public int Id { get; set; }
    public string OrderDetails { get; set; } //serialize the session hashset so this object appears as a json in the db
    public string OrderDate { get; set; }
    public string DeliveryAddress { get; set; }
    public string Recipient { get; set; }
    public string PhoneNumber { get; set; }

    [Precision(6, 2)] public decimal? TotalPrice { get; set; }

    public string UserId { get; set; }
    public IdentityUser User { get; set; }
}