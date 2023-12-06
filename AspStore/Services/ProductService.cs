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

    public async Task<bool> UploadImage(IFormFile file, int? oldId)
    {
        if (oldId != null && _dbContext.ProductsImages.Any(i => i.Name == file.FileName) == false)
        {
            string uploadPath = @$".\wwwroot\images\products\{file.FileName}";
            string filePath = @$"/images/products/{file.FileName}";
            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                var entity = _dbContext.ProductsImages.FirstOrDefault(i=>i.Id == oldId);
                entity.Name = file.FileName;
                entity.ImagePath = filePath;
                await _dbContext.SaveChangesAsync();
            }
            return true;
        }
        if (oldId == null && _dbContext.ProductsImages.Any(i => i.Name == file.FileName) == false)
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

    public async Task<bool> Edit(ProductModel model, IFormFile image, int id)
    {
        var entity = _dbContext.Products.FirstOrDefault(e => e.Id == id);
        //if image is not null null upload a new image and update the old record's info (image name and path)
        if (image != null)
        {
            if (entity != null)
            {
                var newEntity = new ProductModel()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    ProductImageId = entity.ProductImageId,
                    ProductCategoryId = model.ProductCategoryId
                };
                string oldImageName = _dbContext.ProductsImages.FirstOrDefault(i => i.Id == entity.ProductImageId).Name;
                if (await UploadImage(image, entity.ProductImageId) == false)
                {
                    return false;
                }
                DeleteImageFromServer(oldImageName);
                _dbContext.Products.Remove(entity);
                await _dbContext.SaveChangesAsync();
                await _dbContext.AddAsync(newEntity);
                await _dbContext.SaveChangesAsync();
            }
        }
        else
        {
            if (entity != null)
            {
                var newEntity = new ProductModel()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    ProductImageId = entity.ProductImageId,
                    ProductCategoryId = model.ProductCategoryId
                };
                _dbContext.Products.Remove(entity);
                await _dbContext.SaveChangesAsync();
                await _dbContext.AddAsync(newEntity);
                await _dbContext.SaveChangesAsync();
            }
        }

        return true;
    }

    public List<ProductModel> GetProductsByCategory(int categoryId)
    {
        var products = _dbContext.Products.Where(p=>p.ProductCategoryId==categoryId).ToList();
        foreach (var product in products)
        {
            product.ProductImage = _dbContext.ProductsImages.FirstOrDefault(i => i.Id == product.ProductImageId);
        }
        return products;
    }

    private void DeleteImageFromServer(string imageName)
    {
        File.Delete($"./wwwroot/images/products/{imageName}");
    }
}