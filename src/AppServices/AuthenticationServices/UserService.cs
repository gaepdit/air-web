using AirWeb.Domain.Identity;
using GaEpd.AppLibrary.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AirWeb.AppServices.AuthenticationServices;

public interface IUserService : IDisposable
{
    Task<ApplicationUser?> GetCurrentUserAsync();
    Task<ApplicationUser> GetUserAsync(string id);
    Task<ApplicationUser?> FindUserAsync(string id);
    ClaimsPrincipal? GetCurrentPrincipal();
    Task<bool> UserIsInRoleAsync(ApplicationUser user, string role);
}

public sealed class UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    : IUserService
{
    public async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var principal = GetCurrentPrincipal();
        return principal is null ? null : await userManager.GetUserAsync(principal).ConfigureAwait(false);
    }

    public async Task<ApplicationUser> GetUserAsync(string id) =>
        await FindUserAsync(id).ConfigureAwait(false) ?? throw new EntityNotFoundException<ApplicationUser>(id);

    public Task<ApplicationUser?> FindUserAsync(string id) => userManager.FindByIdAsync(id);

    public ClaimsPrincipal? GetCurrentPrincipal() => httpContextAccessor.HttpContext?.User;

    public Task<bool> UserIsInRoleAsync(ApplicationUser user, string role) => userManager.IsInRoleAsync(user, role);

    public void Dispose() => userManager.Dispose();
}
