using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspStore.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace AspStore.Models.Product;

public class ProductModel
{
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Range(1, int.MaxValue)]
    public int? Id { get; set; }

    [Required] public string Name { get; set; }

    [Required] public string Description { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    [Precision(6, 2)]
    [Range(1D, 100000D, ErrorMessage = "Price should be in the range 1 - 100.000")]
    public decimal? Price { get; set; }

    [NotMapped]
    [Required(ErrorMessage = "Please select an image for the product")]
    [DataType(DataType.Upload)]
    [AllowedExtensions(new[] { ".jpg", ".png" })]
    public IFormFile Image { get; set; }

    public int ProductImageId { get; set; }

    [ValidateNever] public ProductImageModel ProductImage { get; set; }

    [Required] [DisplayName("Category")] public int ProductCategoryId { get; set; }

    [ValidateNever] public ProductCategoryModel ProductCategory { get; set; }
}