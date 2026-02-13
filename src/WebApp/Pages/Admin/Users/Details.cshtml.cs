using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.AppServices.Core.EntityServices.Staff.Dto;
using AirWeb.Core.AppRoles;

namespace AirWeb.WebApp.Pages.Admin.Users;

[Authorize(Policy = nameof(Policies.ViewUsersPage))]
public class DetailsModel : PageModel
{
    public StaffViewDto DisplayStaff { get; private set; } = null!;
    public string? OfficeName => DisplayStaff.Office?.Name;
    public IReadOnlyList<AppRole> Roles { get; private set; } = null!;
    public bool CanEdit { get; private set; }

    public async Task<IActionResult> OnGetAsync(
        [FromServices] IStaffService staffService,
        [FromServices] IAuthorizationService authorization,
        string? id)
    {
        if (id is null) return RedirectToPage("Index");
        var staff = await staffService.FindAsync(id);
        if (staff is null) return NotFound();

        DisplayStaff = staff;
        Roles = await staffService.GetAppRolesAsync(DisplayStaff.Id);
        CanEdit = DisplayStaff.Email != null && await authorization.Succeeded(User, Policies.UserAdministrator);

        return Page();
    }
}
