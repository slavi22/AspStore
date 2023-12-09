using System.Security.Claims;
using AspStore.Models.Account;

namespace AspStore.Services.Interfaces;

public interface IUserPageService
{
    public void AddAddress(AddressModel model, ClaimsPrincipal user);
    public void EditAddress(AddressModel model);
    public void DeleteAddress(int id);
}