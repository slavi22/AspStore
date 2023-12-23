using AspStore.Models.Product;
using AspStore.Pagination;

namespace AspStore.Services.Interfaces;

public interface ISearchService
{
    Task<PaginatedList<ProductModel>> Search(string searchString, int? page);
}