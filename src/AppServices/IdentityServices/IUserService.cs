using AirWeb.Domain.Identity;
using System.Security.Claims;

namespace AirWeb.AppServices.IdentityServices;

public interface IUserService
{
    public Task<ApplicationUser?> GetCurrentUserAsync();
    public Task<ApplicationUser> GetUserAsync(string id);
    public Task<ApplicationUser?> FindUserAsync(string id);
    public ClaimsPrincipal? GetCurrentPrincipal();
}
