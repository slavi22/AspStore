using AspStore.Data;
using AspStore.Models.Account;
using AspStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspStore.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly IOrderHistoryService _orderHistoryService;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserPageService _userPageService;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        AppDbContext dbContext, IUserPageService userPageService, IOrderHistoryService orderHistoryService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _dbContext = dbContext;
        _userPageService = userPageService;
        _orderHistoryService = orderHistoryService;
    }

    public IActionResult Register()
    {
        if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            if (!_dbContext.Users.Any())
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(user, new[] { "Admin", "User" });
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var item in result.Errors) ModelState.AddModelError("", item.Description);
            }
            else
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var item in result.Errors) ModelState.AddModelError("", item.Description);
            }
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("SessionData");
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Login()
    {
        if (_dbContext.Users.Any() == false) return RedirectToAction("Register");

        if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "Home");
            }

            var userFound = await _userManager.FindByEmailAsync(model.Email);
            if (userFound == null)
                ModelState.AddModelError("", "User not found!");
            else
                ModelState.AddModelError("", "Invalid Login attempt");
        }

        return View();
    }

    [Authorize(Roles = "Admin")]
    [Route("/Account/Admin")]
    public IActionResult AdminPage()
    {
        return View();
    }

    [Authorize(Roles = "User")]
    [Route("/Account/User")]
    public IActionResult UserPage()
    {
        return View();
    }

    [Authorize(Roles = "User")]
    [Route("/Account/User/Addresses")]
    public IActionResult Addresses()
    {
        var userId = _userManager.GetUserId(User);
        var addresses = _dbContext.UserAddress.Where(i => i.UserId == userId).ToList();
        if (addresses.Any() == false) return RedirectToAction("AddAddress");

        return View(addresses);
    }

    [Authorize(Roles = "User")]
    [Route("/Account/User/Address/New")]
    public IActionResult AddAddress()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "User")]
    [Route("/Account/User/Address/New")]
    public IActionResult AddAddress(AddressModel model, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            _userPageService.AddAddress(model, User);
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Addresses");
        }

        return View();
    }

    [Authorize(Roles = "User")]
    [Route("/Account/User/Address/Delete")]
    [HttpDelete]
    public IActionResult DeleteAddress(int id)
    {
        var address = _dbContext.UserAddress.FirstOrDefault(a => a.Id == id);
        var userId = _userManager.GetUserId(User);
        if (address.UserId != userId) return Unauthorized();

        _userPageService.DeleteAddress(id);
        return Json(Url.Action("Addresses"));
    }

    [Authorize(Roles = "User")]
    [Route("/Account/User/Address/Edit")]
    [HttpPost]
    public IActionResult EditAddress([FromForm] int id)
    {
        var model = _dbContext.UserAddress.FirstOrDefault(a => a.Id == id);
        return View(model);
    }


    [Authorize(Roles = "User")]
    [Route("/Account/User/Address/Edit/{shortName}")]
    [HttpPost]
    public IActionResult EditAddress(AddressModel model, string shortName)
    {
        var address = _dbContext.UserAddress.FirstOrDefault(a => a.Id == model.Id);
        var userId = _userManager.GetUserId(User);
        if (address.UserId != userId) return Unauthorized();

        if (ModelState.IsValid)
        {
            _userPageService.EditAddress(model);
            return RedirectToAction("Addresses");
        }

        return View("EditAddress", model);
    }

    [Authorize(Roles = "User")]
    [Route("/Account/User/Orders")]
    public IActionResult Orders(int? page)
    {
        var paginatedList = _orderHistoryService.OrderPage(page);
        if (page > paginatedList.TotalPages) return NotFound();

        return View(paginatedList);
    }

    [Authorize(Roles = "User")]
    [Route("/Account/User/Order/{id}")]
    public IActionResult OrderDetails(int id)
    {
        var model = _orderHistoryService.OrderDetailsPage(id);
        return View(model);
    }
}