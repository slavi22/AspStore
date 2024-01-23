using AspStore.Pagination;
using AspStore.ViewModels;

namespace AspStore.Services.Interfaces;

public interface IOrderHistoryService
{
    public PaginatedList<OrderHistoryViewModel> OrderPage(int? page);
    public OrderHistoryDetailsViewModel OrderDetailsPage(int id);
}