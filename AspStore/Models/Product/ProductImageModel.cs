using System.ComponentModel.DataAnnotations.Schema;

namespace AspStore.Models.Product;

public class ProductImageModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    public string Name { get; set; }
    public string ImagePath { get; set; }
    public ICollection<ProductModel> Products { get; }
}