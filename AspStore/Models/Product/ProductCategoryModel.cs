using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspStore.Models.Product;

public class ProductCategoryModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<ProductModel> Products { get; }

    public ProductCategoryModel(string name)
    {
        Name = name;
    }
}