using AspStore.Models.Product;

namespace AspStore.Services.Interfaces;

public interface IProductService
{
    public Task Add(ProductModel product);
    public Task<bool> UploadImage(IFormFile file, int? oldId);
    public bool Remove(int id);
    public Task<bool> Edit(ProductModel product, IFormFile image, int id);
    public List<ProductModel> GetProductsByCategory(int categoryId);
}