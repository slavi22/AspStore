using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspStore.Data;
using AspStore.Extensions;
using AspStore.Models.Errors;
using AspStore.Models.Product;
using AspStore.Pagination;
using AspStore.Services.Interfaces;
using AspStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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
            return View();
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