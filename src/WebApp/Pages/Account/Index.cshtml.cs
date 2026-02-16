using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.AppServices.Core.EntityServices.Staff.Dto;
using AirWeb.Core.AppRoles;
using AirWeb.WebApp.Models;

namespace AirWeb.WebApp.Pages.Account;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class AccountIndexModel : PageModel
{
    public StaffViewDto DisplayStaff { get; private set; } = null!;
    public string? OfficeName => DisplayStaff.Office?.Name;
    public IReadOnlyList<AppRole> Roles { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync([FromServices] IStaffService staffService)
    {
        DisplayStaff = await staffService.GetCurrentUserAsync();
        Roles = await staffService.GetAppRolesAsync(DisplayStaff.Id);

        if (DisplayStaff.Office is null)
            TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning,
                message: "Your Office must be set. Select “Edit Profile” and choose an Office from the list.");

        return Page();
    }
}
