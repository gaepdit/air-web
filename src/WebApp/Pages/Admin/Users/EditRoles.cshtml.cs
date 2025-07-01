using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.Identity;
using AirWeb.WebApp.Models;

namespace AirWeb.WebApp.Pages.Admin.Users;

[Authorize(Policy = nameof(Policies.UserAdministrator))]
public class EditRolesModel(IStaffService staffService) : PageModel
{
    [BindProperty]
    public string UserId { get; set; } = string.Empty;

    [BindProperty]
    public List<RoleSetting> RoleSettings { get; set; } = [];

    public StaffViewDto DisplayStaff { get; private set; } = null!;
    public string? OfficeName => DisplayStaff.Office?.Name;

    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (id is null) return RedirectToPage("Index");
        var staff = await staffService.FindAsync(id);
        if (staff is null) return NotFound();

        DisplayStaff = staff;
        UserId = id;

        await PopulateRoleSettingsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var rolesDictionary = RoleSettings.ToDictionary(setting => setting.Name, setting => setting.IsSelected);
        var result = await staffService.UpdateRolesAsync(UserId, rolesDictionary);

        if (result.Succeeded)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "User roles successfully updated.");
            return RedirectToPage("Details", new { id = UserId });
        }

        foreach (var err in result.Errors)
            ModelState.AddModelError(string.Empty, string.Concat(err.Code, ": ", err.Description));

        var staff = await staffService.FindAsync(UserId);
        if (staff is null) return BadRequest();

        DisplayStaff = staff;

        return Page();
    }

    private async Task PopulateRoleSettingsAsync()
    {
        var roles = await staffService.GetRolesAsync(DisplayStaff.Id);

        RoleSettings.AddRange(AppRole.AllRoles!.Select(pair => new RoleSetting
        {
            Name = pair.Key,
            Category = pair.Value.Category,
            DisplayName = pair.Value.DisplayName,
            Description = pair.Value.Description,
            IsSelected = roles.Contains(pair.Key),
        }));
    }

    public class RoleSetting
    {
        public required string Name { get; init; }
        public required string Category { get; init; }
        public required string DisplayName { get; init; }
        public required string Description { get; init; }
        public bool IsSelected { get; init; }
    }
}
