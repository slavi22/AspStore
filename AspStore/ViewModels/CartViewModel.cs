using System.Text.Json.Serialization;
using AspStore.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace AspStore.ViewModels;

public class CartViewModel
{
    public int Quantity { get; set; }
    public ProductModel Product { get; set; }
    [Precision(6, 2)]
    public decimal? InitialPrice { get; set; }
    [Precision(6, 2)]
    public decimal? Price { get; set; }
}