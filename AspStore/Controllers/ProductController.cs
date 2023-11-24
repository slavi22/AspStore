using AspStore.Data;
using AspStore.Models.Product;
using AspStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspStore.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly AppDbContext _dbContext;

    public ProductController(IProductService productService, AppDbContext dbContext)
    {
        _productService = productService;
        _dbContext = dbContext;
    }
    /*public IActionResult Index()
    {
        //var model = _productService.GetAllProducts().Where(e => e.ProductCategoryId == 2);
        var model = _productService.GetAllProducts();
        return View(model);
    }*/

    [Route("/Products/Index")]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [Route("/Product/Add")]
    public IActionResult Add()
    {
        var productsCategory = _dbContext.ProductsCategory.ToList();
        ViewData["ProductCategories"] = new SelectList(productsCategory, "Id", "Name");
        return View();
    }

    //[Route("[controller]/[action]/{product}")]
    [Route("/Product/Add/{product}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Add(string product)
    {
        if (product == "Cpu")
        {
            ViewData["AddText"] = "Add a new CPU product";
            var productsCategory = _dbContext.ProductsCategory.Where(p => p.Id == 1).ToList();
            ViewData["ProductCategories"] = new SelectList(productsCategory, "Id", "Name");
            return View();
        }
        else if (product == "Gpu")
        {
            ViewData["AddText"] = "Add a new GPU product";
            var productsCategory = _dbContext.ProductsCategory.Where(p => p.Id == 2).ToList();
            ViewData["ProductCategories"] = new SelectList(productsCategory, "Id", "Name");
            return View();
        }

        else if (product == "Ram")
        {
            ViewData["AddText"] = "Add a new RAM product";
            var productsCategory = _dbContext.ProductsCategory.Where(p => p.Id == 3).ToList();
            ViewData["ProductCategories"] = new SelectList(productsCategory, "Id", "Name");
            return View();
        }

        else if (product == "Motherboard")
        {
            ViewData["AddText"] = "Add a new Motherboard product";
            var productsCategory = _dbContext.ProductsCategory.Where(p => p.Id == 4).ToList();
            ViewData["ProductCategories"] = new SelectList(productsCategory, "Id", "Name");
            return View();
        }

        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    [Route("/Product/Add")]
    [Route("Product/Add/{product}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Add(ProductModel model)
    {
        var productsCategory = _dbContext.ProductsCategory.ToList();
        ViewData["ProductCategories"] = new SelectList(productsCategory, "Id", "Name");
        if (ModelState.IsValid)
        {
            if (_dbContext.Products.Any(id => id.Id == model.Id) == true)
            {
                ModelState.AddModelError("Id", $"Product with id code - \"{model.Id}\" already exists in the database!");
                return View();
            }
            else
            {
                if (await _productService.UploadImage(model.Image) == false)
                {
                    ModelState.AddModelError("Image",
                        $"Image with the name - \"{model.Image.FileName}\" already exists in the database!");
                    return View();
                }
                else
                {
                    await _productService.Add(model);
                    if (model.ProductCategoryId == 1)
                    {
                        return RedirectToAction("Cpu");
                    }
                    else if (model.ProductCategoryId == 2)
                    {
                        return RedirectToAction("Gpu");
                    }
                    else if (model.ProductCategoryId == 3)
                    {
                        return RedirectToAction("Ram");
                    }
                    else if (model.ProductCategoryId == 4)
                    {
                        return RedirectToAction("Motherboard");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
        }

        return View();
    }

    public IActionResult Cpu()
    {
        var products = _productService.GetProductsByCategory(1);
        return View(products);
    }

    [Route("/Product/Cpu/{id}")]
    public IActionResult Cpu(int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id && p.ProductCategoryId == 1);
        if (product != null)
        {
            product.ProductImage = _dbContext.ProductsImages.Where(i => i.Id == product.Id).FirstOrDefault();
            return View("Product", product);
        }
        else
        {
            return NotFound();
        }
    }

    public IActionResult Gpu()
    {
        var products = _productService.GetProductsByCategory(2);
        return View(products);
    }
    
    [Route("/Product/Gpu/{id}")]
    public IActionResult Gpu(int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id && p.ProductCategoryId == 2);
        if (product != null)
        {
            product.ProductImage = _dbContext.ProductsImages.Where(i => i.Id == product.Id).FirstOrDefault();
            return View("Product", product);
        }
        else
        {
            return NotFound();
        }
    }

    public IActionResult Ram()
    {
        var products = _productService.GetProductsByCategory(3);
        return View(products);
    }
    
    [Route("/Product/Ram/{id}")]
    public IActionResult Ram(int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id && p.ProductCategoryId == 3);
        if (product != null)
        {
            product.ProductImage = _dbContext.ProductsImages.Where(i => i.Id == product.Id).FirstOrDefault();
            return View("Product", product);
        }
        else
        {
            return NotFound();
        }
    }

    public IActionResult Motherboard()
    {
        var products = _productService.GetProductsByCategory(4);
        return View(products);
    }
    
    [Route("/Product/Motherboard/{id}")]
    public IActionResult Motherboard(int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id && p.ProductCategoryId == 4);
        if (product != null)
        {
            product.ProductImage = _dbContext.ProductsImages.Where(i => i.Id == product.Id).FirstOrDefault();
            return View("Product", product);
        }
        else
        {
            return NotFound();
        }
    }
}