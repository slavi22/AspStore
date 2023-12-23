using AspStore.Data;
using AspStore.Models.Product;
using AspStore.Pagination;
using AspStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspStore.Services;

public class SearchService : ISearchService
{
    private readonly AppDbContext _dbContext;
    public SearchService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<PaginatedList<ProductModel>> Search(string searchString, int? page)
    {
        var products = await _dbContext.Products.Where(p => p.Name.Contains(searchString)).ToListAsync();
        foreach (var product in products)
        {
            product.ProductImage = _dbContext.ProductsImages.FirstOrDefault(i => i.Id == product.ProductImageId);
        }
        var paginatedList = PaginatedList<ProductModel>.Create(products, page ?? 1, 8);
        return paginatedList;
    }
}