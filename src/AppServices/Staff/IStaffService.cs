using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.Identity;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Identity;

namespace AirWeb.AppServices.Staff;

public interface IStaffService : IDisposable, IAsyncDisposable
{
    Task<StaffViewDto> GetCurrentUserAsync();
    Task<StaffViewDto?> FindAsync(string id);
    Task<IPaginatedResult<StaffSearchResultDto>> SearchAsync(StaffSearchDto spec, PaginatedRequest paging);
    Task<IReadOnlyList<ListItem<string>>> GetUsersAsync(bool includeInactive = false);
    Task<IReadOnlyList<ListItem<string>>> GetUsersInRoleAsync(AppRole role);
    Task<IList<string>> GetRolesAsync(string id);
    Task<IReadOnlyList<AppRole>> GetAppRolesAsync(string id);
    Task<IdentityResult> UpdateRolesAsync(string id, Dictionary<string, bool> roles);
    Task<IdentityResult> UpdateAsync(string id, StaffUpdateDto resource);
}
