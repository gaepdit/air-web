using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.AppServices.Core.Caching;
using AirWeb.AppServices.Core.EntityServices.Staff.Dto;
using AirWeb.AppServices.Core.EntityServices.Staff.Exceptions;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Domain.Core.AppRoles;
using AirWeb.Domain.Core.Entities;
using AutoMapper;
using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;

namespace AirWeb.AppServices.Core.EntityServices.Staff;

public interface IStaffService : IDisposable, IAsyncDisposable
{
    Task<StaffViewDto> GetCurrentUserAsync();
    Task<StaffViewDto?> FindAsync(string id);
    Task<StaffViewDto?> FindByEmailAsync(string? email);
    Task<IPaginatedResult<StaffSearchResultDto>> SearchAsync(StaffSearchDto spec, PaginatedRequest paging);
    Task<IReadOnlyList<ListItem<string>>> GetAllStaffAsync(CancellationToken token = default);
    Task<IReadOnlyList<ListItem<string>>> GetStaffInRoleAsync(CancellationToken token, params AppRole[] roles);
    Task<bool> IsInRoleAsync(string id, params AppRole[] roles);
    Task<IList<string>> GetRolesAsync(string id);
    Task<IReadOnlyList<AppRole>> GetAppRolesAsync(string id);
    Task<IdentityResult> UpdateRolesAsync(string id, Dictionary<string, bool> roles);
    Task<IdentityResult> UpdateAsync(string id, StaffUpdateDto resource);
}

public sealed class StaffService(
    IUserService userService,
    UserManager<ApplicationUser> userManager,
    IMapper mapper,
    IOfficeRepository officeRepository,
    IAuthorizationService authorization,
    HybridCache cache,
    ILogger<StaffService> logger) : IStaffService
{
    public async Task<StaffViewDto> GetCurrentUserAsync()
    {
        var user = await userService.GetCurrentUserAsync().ConfigureAwait(false)
                   ?? throw new CurrentUserNotFoundException();
        return mapper.Map<StaffViewDto>(user);
    }

    public async Task<StaffViewDto?> FindAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id).ConfigureAwait(false);
        return mapper.Map<StaffViewDto?>(user);
    }

    public async Task<StaffViewDto?> FindByEmailAsync(string? email)
    {
        if (email is null) return null;
        var user = await userManager.FindByEmailAsync(email).ConfigureAwait(false);
        return mapper.Map<StaffViewDto?>(user);
    }

    public async Task<IPaginatedResult<StaffSearchResultDto>> SearchAsync(StaffSearchDto spec, PaginatedRequest paging)
    {
        var users = string.IsNullOrEmpty(spec.Role)
            ? userManager.Users.ApplyFilter(spec)
            : (await userManager.GetUsersInRoleAsync(spec.Role).ConfigureAwait(false)).AsQueryable().ApplyFilter(spec);
        var list = users.Skip(paging.Skip).Take(paging.Take);
        var listMapped = mapper.Map<IReadOnlyList<StaffSearchResultDto>>(list);

        return new PaginatedResult<StaffSearchResultDto>(listMapped, users.Count(), paging);
    }

    private const string CachedStaffLists = nameof(CachedStaffLists);
    private const string AllUsersCache = nameof(AllUsersCache);

    public async Task<IReadOnlyList<ListItem<string>>> GetAllStaffAsync(CancellationToken token = default) =>
        await cache.GetOrCreateAsync(AllUsersCache, factory: _ =>
                    Task.FromResult(mapper.Map<IReadOnlyList<StaffViewDto>>(userManager.Users)
                        .Select(e => new ListItem<string>(e.Id, e.SortableNameWithOffice))
                        .OrderBy(e => e.Name).ToList()),
                expiration: CacheConstants.StaffServiceCacheTime, logger, tag: CachedStaffLists, token)
            .ConfigureAwait(false);

    public async Task<IReadOnlyList<ListItem<string>>> GetStaffInRoleAsync(CancellationToken token,
        params AppRole[] roles) =>
        await cache.GetOrCreateAsync($"StaffList.{roles.Select(r => r.Name).OrderBy(n => n).ConcatWithSeparator("+")}",
                factory: async _ => await GetStaffInRoleFromStore(roles).ConfigureAwait(false),
                expiration: CacheConstants.StaffServiceCacheTime, logger, tag: CachedStaffLists, token)
            .ConfigureAwait(false);

    private async Task<List<ListItem<string>>> GetStaffInRoleFromStore(AppRole[] roles)
    {
        IList<IList<ApplicationUser>> userSets = [];
        foreach (var appRole in roles)
            userSets.Add(await userManager.GetUsersInRoleAsync(appRole.Name).ConfigureAwait(false));

        var users = userSets.Aggregate((current, union) => current.Union(union).ToList());

        return mapper.Map<IReadOnlyList<StaffViewDto>>(users)
            .Where(user => user.Active)
            .Select(user => new ListItem<string>(user.Id, user.SortableNameWithOffice))
            .OrderBy(e => e.Name).ToList();
    }

    public async Task<bool> IsInRoleAsync(string id, params AppRole[] roles)
    {
        var user = await userManager.FindByIdAsync(id).ConfigureAwait(false);
        return user is not null && await userService.IsInRoleAsync(user, roles.Select(role => role.Name).ToArray())
            .ConfigureAwait(false);
    }

    public async Task<IList<string>> GetRolesAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id).ConfigureAwait(false);
        return user is null ? [] : await userManager.GetRolesAsync(user).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<AppRole>> GetAppRolesAsync(string id) =>
        AppRole.RolesAsAppRoles(await GetRolesAsync(id).ConfigureAwait(false))
            .OrderBy(r => r.DisplayName).ToList();

    public async Task<IdentityResult> UpdateRolesAsync(string id, Dictionary<string, bool> roles)
    {
        var principal = userService.GetCurrentPrincipal()!;
        if (!await authorization.Succeeded(principal, Policies.UserAdministrator).ConfigureAwait(false))
            throw new InsufficientPermissionsException(nameof(Policies.UserAdministrator));

        var user = await userManager.FindByIdAsync(id).ConfigureAwait(false)
                   ?? throw new EntityNotFoundException<ApplicationUser>(id);

        foreach (var (role, value) in roles)
        {
            var result = await UpdateUserRoleAsync(user, role, value).ConfigureAwait(false);
            if (result != IdentityResult.Success) return result;
        }

        await cache.RemoveByTagAsync(CachedStaffLists).ConfigureAwait(false);
        return IdentityResult.Success;

        async Task<IdentityResult> UpdateUserRoleAsync(ApplicationUser u, string r, bool addToRole)
        {
            var isInRole = await userManager.IsInRoleAsync(u, r).ConfigureAwait(false);
            if (addToRole == isInRole) return IdentityResult.Success;

            return addToRole switch
            {
                true => await userManager.AddToRoleAsync(u, r).ConfigureAwait(false),
                false => await userManager.RemoveFromRoleAsync(u, r).ConfigureAwait(false),
            };
        }
    }

    public async Task<IdentityResult> UpdateAsync(string id, StaffUpdateDto resource)
    {
        var principal = userService.GetCurrentPrincipal()!;
        if (id != principal.GetNameIdentifierId() &&
            !await authorization.Succeeded(principal, Policies.UserAdministrator).ConfigureAwait(false))
        {
            throw new InsufficientPermissionsException(nameof(Policies.UserAdministrator));
        }

        var user = await userManager.FindByIdAsync(id).ConfigureAwait(false)
                   ?? throw new EntityNotFoundException<ApplicationUser>(id);

        user.PhoneNumber = resource.PhoneNumber;
        user.Office = resource.OfficeId is null
            ? null
            : await officeRepository.GetAsync(resource.OfficeId.Value).ConfigureAwait(false);
        user.Active = resource.Active;
        user.ProfileUpdatedAt = DateTimeOffset.UtcNow;

        await cache.RemoveByTagAsync(CachedStaffLists).ConfigureAwait(false);
        return await userManager.UpdateAsync(user).ConfigureAwait(false);
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose()
    {
        userManager.Dispose();
        officeRepository.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        userManager.Dispose();
        await officeRepository.DisposeAsync().ConfigureAwait(false);
    }

    #endregion
}
