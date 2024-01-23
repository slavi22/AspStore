using System.Diagnostics;
using AspStore.Data;
using AspStore.Models.Errors;
using AspStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspStore.Controllers
{
    [Authorize(Policy = "FirstTimeSetupComplete")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _dbContext;
        private readonly ISearchService _searchService;

        public HomeController(ILogger<HomeController> logger, AppDbContext dbContext, ISearchService searchService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _searchService = searchService;
        }

        public IActionResult Index()
        {
            var latestProducts = _dbContext.Products.OrderByDescending(i => i.Id).Take(5).ToList();
            ViewData["actions"] = new List<string>() { "Cpu", "Gpu", "Ram", "Motherboard" };
            foreach (var item in latestProducts)
            {
                item.ProductImage = _dbContext.ProductsImages.FirstOrDefault(i => i.Id == item.ProductImageId);
            }
            return View(latestProducts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Search(string searchString, int? page)
        {
            ViewData["actions"] = new List<string>() { "Cpu", "Gpu", "Ram", "Motherboard" };
            var paginatedList = await _searchService.Search(searchString, page);
            if (page > paginatedList.TotalPages)
            {
                return NotFound();
            }
            else
            {
                return View("../Search/Search", paginatedList);
            }
        }
    }
}