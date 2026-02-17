using AirWeb.AppServices.Core.EntityServices.Staff.Dto;
using AirWeb.Domain.Core.AppRoles;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Identity;

namespace AirWeb.AppServices.Core.EntityServices.Staff;

public interface IStaffService : IDisposable, IAsyncDisposable
{
    Task<StaffViewDto> GetCurrentUserAsync();
    Task<StaffViewDto?> FindAsync(string id);
    Task<StaffViewDto?> FindByEmailAsync(string? email);
    Task<IPaginatedResult<StaffSearchResultDto>> SearchAsync(StaffSearchDto spec, PaginatedRequest paging);
    Task<IReadOnlyList<ListItem<string>>> GetUsersAsync(bool includeInactive = false);
    Task<IReadOnlyList<ListItem<string>>> GetUsersInRoleAsync(AppRole role);
    Task<bool> IsInRoleAsync(string id, AppRole role);
    Task<IList<string>> GetRolesAsync(string id);
    Task<IReadOnlyList<AppRole>> GetAppRolesAsync(string id);
    Task<IdentityResult> UpdateRolesAsync(string id, Dictionary<string, bool> roles);
    Task<IdentityResult> UpdateAsync(string id, StaffUpdateDto resource);
}
