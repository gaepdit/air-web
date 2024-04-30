using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Identity;
using MyApp.AppServices.Staff.Dto;
using MyApp.Domain.Identity;

namespace MyApp.AppServices.Staff;

public interface IStaffService : IDisposable, IAsyncDisposable
{
    Task<StaffViewDto> GetCurrentUserAsync();
    Task<StaffViewDto?> FindAsync(string id);
    Task<IReadOnlyList<StaffViewDto>> GetListAsync(StaffSearchDto spec);
    Task<IPaginatedResult<StaffSearchResultDto>> SearchAsync(StaffSearchDto spec, PaginatedRequest paging);
    Task<IReadOnlyList<ListItem<string>>> GetAsListItemsAsync(bool includeInactive = false);
    Task<IReadOnlyList<ListItem<string>>> GetUsersInRoleAsListItemsAsync(AppRole role, Guid officeId);
    Task<IList<string>> GetRolesAsync(string id);
    Task<IReadOnlyList<AppRole>> GetAppRolesAsync(string id);
    Task<bool> HasAppRoleAsync(string id, AppRole role);
    Task<IdentityResult> UpdateRolesAsync(string id, Dictionary<string, bool> roles);
    Task<IdentityResult> UpdateAsync(string id, StaffUpdateDto resource);
}
