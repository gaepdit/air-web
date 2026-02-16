using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.AppServices.Core.EntityServices.Staff.Dto;
using AirWeb.AppServices.Core.EntityServices.Staff.Exceptions;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Core.AppRoles;
using AirWeb.Core.Entities;
using AutoMapper;
using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Web;

namespace AirWeb.AppServices.Core.EntityServices.Staff;

public sealed class StaffService(
    IUserService userService,
    UserManager<ApplicationUser> userManager,
    IMapper mapper,
    IOfficeRepository officeRepository,
    IAuthorizationService authorization)
    : IStaffService
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

    public Task<IReadOnlyList<ListItem<string>>> GetUsersAsync(bool includeInactive = false)
    {
        var status = includeInactive ? SearchStaffStatus.All : SearchStaffStatus.Active;
        var spec = new StaffSearchDto { Status = status };
        var users = userManager.Users.ApplyFilter(spec);
        return Task.FromResult<IReadOnlyList<ListItem<string>>>(mapper.Map<IReadOnlyList<StaffViewDto>>(users)
            .Select(e => new ListItem<string>(e.Id, e.SortableNameWithOffice))
            .ToList());
    }

    public async Task<IReadOnlyList<ListItem<string>>> GetUsersInRoleAsync(AppRole role) =>
        (await userManager.GetUsersInRoleAsync(role.Name).ConfigureAwait(false))
        .Where(user => user.Active)
        .Select(user => new ListItem<string>(user.Id, user.SortableFullName))
        .ToList();

    public async Task<bool> IsInRoleAsync(string id, AppRole role)
    {
        var user = await userManager.FindByIdAsync(id).ConfigureAwait(false);
        return user is not null && await userManager.IsInRoleAsync(user, role.Name).ConfigureAwait(false);
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
