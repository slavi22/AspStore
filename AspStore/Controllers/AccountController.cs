using AspStore.Data;
using AspStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspStore.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppDbContext _dbContext;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (_dbContext.Users.Count()<1)
                {
                    IdentityUser user = new IdentityUser() { UserName = model.Email, Email = model.Email };
                    IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRolesAsync(user, new[] { "Admin", "User" });
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
                else
                {
                    IdentityUser user = new IdentityUser() { UserName = model.Email, Email = model.Email };
                    IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            if (_dbContext.Users.Any() == false)
            {
                return RedirectToAction("Register");
            }
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
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                var userFound = await _userManager.FindByEmailAsync(model.Email);
                if (userFound == null)
                {
                    ModelState.AddModelError("", "User not found!");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Login attempt");
                }
            }
            return View();
        }
}