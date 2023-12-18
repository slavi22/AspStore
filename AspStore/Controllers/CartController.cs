using AspStore.Data;
using AspStore.Extensions;
using AspStore.Migrations;
using AspStore.Services.Interfaces;
using AspStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AspStore.Controllers;

public class CartController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly ICartService _cartService;
    private readonly UserManager<IdentityUser> _userManager;

    public CartController(AppDbContext dbContext, ICartService cartService, UserManager<IdentityUser> userManager)
    {
        _dbContext = dbContext;
        _cartService = cartService;
        _userManager = userManager;
    }

    [Authorize]
    [Route("/Cart")]
    public IActionResult Cart()
    {
        var model = HttpContext.Session.GetComplexData<HashSet<CartViewModel>>("Cart");
        foreach (var entity in model)
        {
            entity.Product.ProductImage =
                _dbContext.ProductsImages.FirstOrDefault(i => i.Id == entity.Product.ProductImageId);
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult AddToCart(int id)
    {
        _cartService.AddToCart(id);
        string redirectLink = Request.GetTypedHeaders().Referer.ToString();
        return Json(new {url = redirectLink});
    }

    [HttpPost]
    public IActionResult SubtractProductQuantity(int id)
    {
        _cartService.SubtractQuantity(id);
        var model = HttpContext.Session.GetComplexData<HashSet<CartViewModel>>("Cart");
        foreach (var entity in model)
        {
            entity.Product.ProductImage =
                _dbContext.ProductsImages.FirstOrDefault(i => i.Id == entity.Product.ProductImageId);
        }

        return PartialView("Cart", model);
    }

    [HttpPost]
    public IActionResult AddProductQuantity(int id)
    {
        _cartService.AddQuantity(id);
        var model = HttpContext.Session.GetComplexData<HashSet<CartViewModel>>("Cart");
        foreach (var entity in model)
        {
            entity.Product.ProductImage =
                _dbContext.ProductsImages.FirstOrDefault(i => i.Id == entity.Product.ProductImageId);
        }

        return PartialView("Cart", model);
    }

    [HttpPost]
    public async Task<IActionResult> RemoveProductFromCart(int id)
    {
        _cartService.RemoveProduct(id);
        var model = HttpContext.Session.GetComplexData<HashSet<CartViewModel>>("Cart");
        foreach (var entity in model)
        {
            entity.Product.ProductImage =
                _dbContext.ProductsImages.FirstOrDefault(i => i.Id == entity.Product.ProductImageId);
        }

        var partialView = await this.RenderViewAsync("Cart", model, true);
        return Json(new { PartialView = partialView, Quantity = model.Sum(i => i.Quantity) });
    }

    [Route("/Cart/Checkout")]
    public IActionResult Checkout()
    {
        if (HttpContext.User.Identity.IsAuthenticated &&
            HttpContext.Session.GetComplexData<HashSet<CartViewModel>>("Cart").Count == 0)
        {
            return RedirectToAction("Cart");
        }

        var model = new CheckoutViewModel()
        {
            UserAddress = _dbContext.UserAddress.Where(i => i.UserId == _userManager.GetUserId(User)).ToList(),
            Cart = HttpContext.Session.GetComplexData<HashSet<CartViewModel>>("Cart")
        };
        foreach (var entity in model.Cart)
        {
            entity.Product.ProductImage =
                _dbContext.ProductsImages.FirstOrDefault(i => i.Id == entity.Product.ProductImageId);
        }
        return View(model);
    }

    [Route("/Cart/Checkout")]
    [HttpPost]
    public IActionResult Checkout(int addressId)
    {
        var address = _dbContext.UserAddress.FirstOrDefault(a => a.Id == addressId);
        var userId = _userManager.GetUserId(User);
        if (address.UserId != userId)
        {
            return Unauthorized();
        }
        _cartService.PlaceOrder(addressId);
        return Ok();
    }
}