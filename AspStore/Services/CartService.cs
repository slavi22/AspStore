using AspStore.Data;
using AspStore.Extensions;
using AspStore.Models.Cart;
using AspStore.Services.Interfaces;
using AspStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace AspStore.Services;

public class CartService : ICartService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISession _session;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public CartService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor,
        SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _signInManager = signInManager;
        _userManager = userManager;
        _session = _httpContextAccessor.HttpContext.Session;
        if (_signInManager.IsSignedIn(_httpContextAccessor.HttpContext.User) &&
            _session.GetComplexData<HashSet<CartViewModel>>("Cart") == null)
            _session.SetComplexData("Cart", new HashSet<CartViewModel>());
    }

    public void AddToCart(int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);
        var cart = _session.GetComplexData<HashSet<CartViewModel>>("Cart");
        var cartItem = new CartViewModel
            { Product = product, Quantity = 1, Price = product.Price, InitialPrice = product.Price };
        if (cart.Any(i => i.Product.Id == cartItem.Product.Id))
        {
            var duplicateItem = cart.FirstOrDefault(i => i.Product.Id == cartItem.Product.Id);
            duplicateItem.InitialPrice = cartItem.InitialPrice;
            duplicateItem.Quantity += 1;
            duplicateItem.Price = cartItem.Price * duplicateItem.Quantity;
            cart.Add(duplicateItem);
        }
        else
        {
            cart.Add(cartItem);
        }

        _session.SetComplexData("Cart", cart);
    }

    public void SubtractQuantity(int id)
    {
        var cart = _session.GetComplexData<HashSet<CartViewModel>>("Cart");
        var model = cart.FirstOrDefault(i => i.Product.Id == id);
        if (model.Quantity > 1)
        {
            var edittedItem = cart.FirstOrDefault(i => i.Product.Id == model.Product.Id);
            edittedItem.Quantity -= 1;
            edittedItem.Price = model.InitialPrice * edittedItem.Quantity;
            cart.Add(edittedItem);
        }
        else
        {
            cart.Remove(model);
        }

        _session.SetComplexData("Cart", cart);
    }

    public void AddQuantity(int id)
    {
        var cart = _session.GetComplexData<HashSet<CartViewModel>>("Cart");
        var model = cart.FirstOrDefault(i => i.Product.Id == id);
        var edittedItem = cart.FirstOrDefault(i => i.Product.Id == model.Product.Id);
        edittedItem.Quantity += 1;
        edittedItem.Price = model.InitialPrice * edittedItem.Quantity;
        cart.Add(edittedItem);
        _session.SetComplexData("Cart", cart);
    }

    public void RemoveProduct(int id)
    {
        var cart = _session.GetComplexData<HashSet<CartViewModel>>("Cart");
        var model = cart.FirstOrDefault(i => i.Product.Id == id);
        cart.Remove(model);
        _session.SetComplexData("Cart", cart);
    }

    public void PlaceOrder(int addressId)
    {
        var cart = _session.GetComplexData<HashSet<CartViewModel>>("Cart");
        var orderDetails = JsonConvert.SerializeObject(cart.Select(i => new
        {
            i.Quantity, ProductId = i.Product.Id, i.Price
        }));
        var addressModel = _dbContext.UserAddress.FirstOrDefault(i => i.Id == addressId);
        var order = new OrderModel
        {
            OrderDetails = orderDetails,
            OrderDate = DateTime.Now.ToString("dd/MM/yyyy"),
            DeliveryAddress = addressModel.Address,
            Recipient = addressModel.Recipient,
            PhoneNumber = addressModel.PhoneNumber,
            TotalPrice = cart.Sum(i => i.Price),
            UserId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User)
        };
        _dbContext.Orders.Add(order);
        _dbContext.SaveChanges();
    }
}