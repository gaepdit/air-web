using AirWeb.Domain.Identity;
using GaEpd.AppLibrary.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AirWeb.AppServices.UserServices;

public class UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    : IUserService
{
    public async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var principal = GetCurrentPrincipal();
        return principal is null ? null : await userManager.GetUserAsync(principal).ConfigureAwait(false);
    }

    public async Task<ApplicationUser> GetUserAsync(string id) =>
        await FindUserAsync(id).ConfigureAwait(false)
        ?? throw new EntityNotFoundException<ApplicationUser>(id);

    public async Task<ApplicationUser?> FindUserAsync(string? id) =>
        id is null ? null : await userManager.FindByIdAsync(id).ConfigureAwait(false);

    public ClaimsPrincipal? GetCurrentPrincipal() => httpContextAccessor.HttpContext?.User;
}
