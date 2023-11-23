using AspStore.Models.Product;

namespace AspStore.Services.Interfaces;

public interface IProductService
{
    public Task Add(ProductModel product);
    public Task<bool> UploadImage(IFormFile file);
    public bool Remove(int id);
    public void Edit(int id);
    public List<ProductModel> GetProductsByCategory(int categoryId);
}