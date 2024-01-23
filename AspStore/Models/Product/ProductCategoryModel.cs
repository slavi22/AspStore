using System.ComponentModel.DataAnnotations.Schema;

namespace AspStore.Models.Product;

public class ProductCategoryModel
{
    public ProductCategoryModel(string name)
    {
        Name = name;
    }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    public string Name { get; set; }
    public ICollection<ProductModel> Products { get; }
}