﻿using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.Staff;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.Identity;

namespace AirWeb.WebApp.Pages.Admin.Users;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class DetailsModel : PageModel
{
    public StaffViewDto DisplayStaff { get; private set; } = default!;
    public string? OfficeName => DisplayStaff.Office?.Name;
    public IReadOnlyList<AppRole> Roles { get; private set; } = default!;
    public bool IsUserAdministrator { get; private set; }

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
        IsUserAdministrator = await authorization.Succeeded(User, Policies.UserAdministrator);

        return Page();
    }
}
