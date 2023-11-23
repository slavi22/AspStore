using AspStore.Data;
using AspStore.Models.Product;
using AspStore.Services.Interfaces;

namespace AspStore.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _dbContext;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductService(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task Add(ProductModel product)
    {
        var productImage = _dbContext.ProductsImages.FirstOrDefault(i => i.Name == product.Image.FileName);
        product.ProductImageId = productImage.Id;
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> UploadImage(IFormFile file)
    {
        if (_dbContext.ProductsImages.Any(i => i.Name == file.FileName) == false)
        {
            var modelId = _dbContext.ProductsImages.OrderByDescending(i => i.Id).FirstOrDefault();
            int id = 1;
            if (modelId != null)
            {
                id = modelId.Id + 1;
            }

            //string filePath = Path.Combine(_webHostEnvironment.WebRootPath, @$"images\products\{file.FileName}");
            string uploadPath = @$".\wwwroot\images\products\{file.FileName}";
            string filePath = @$"/images/products/{file.FileName}";
            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                var imageModel = new ProductImageModel() { Id = id, ImagePath = filePath, Name = file.FileName };
                await _dbContext.ProductsImages.AddAsync(imageModel);
                await _dbContext.SaveChangesAsync();
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Remove(int id)
    {
        var entity = _dbContext.Products.FirstOrDefault(e => e.Id == id);
        if (entity == null)
        {
            return false;
        }
        else
        {
            _dbContext.Remove(entity);
            return true;
        }
    }

    public void Edit(int id)
    {
        throw new NotImplementedException();
    }

    public List<ProductModel> GetProductsByCategory(int categoryId)
    {
        var products = _dbContext.Products.Where(p=>p.ProductCategoryId==categoryId).ToList();
        foreach (var product in products)
        {
            product.ProductImage = _dbContext.ProductsImages.Where(i => i.Id == product.Id).FirstOrDefault();
        }
        return products;
    }
}