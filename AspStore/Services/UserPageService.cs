using System.Security.Claims;
using AspStore.Data;
using AspStore.Models.Account;
using AspStore.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AspStore.Services;

public class UserPageService : IUserPageService
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public UserPageService(AppDbContext dbContext, UserManager<IdentityUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public void AddAddress(AddressModel model, ClaimsPrincipal user)
    {
        if (_dbContext.UserAddress.Any() == false)
        {
            model.Id = 1;
        }
        else
        {
            var lastRecordId = _dbContext.UserAddress.OrderByDescending(i => i.Id).FirstOrDefault().Id;
            model.Id = lastRecordId + 1;
        }

        var userId = _userManager.GetUserId(user);
        model.UserId = userId;
        _dbContext.UserAddress.Add(model);
        _dbContext.SaveChanges();
    }

    public void EditAddress(AddressModel model)
    {
        var entity = _dbContext.UserAddress.FirstOrDefault(e => e.Id == model.Id);
        entity.ShortName = model.ShortName;
        entity.Recipient = model.Recipient;
        entity.City = model.City;
        entity.Address = model.Address;
        _dbContext.SaveChanges();
    }

    public void DeleteAddress(int id)
    {
        var entity = _dbContext.UserAddress.FirstOrDefault(e => e.Id == id);
        _dbContext.UserAddress.Remove(entity);
        _dbContext.SaveChanges();
    }
}