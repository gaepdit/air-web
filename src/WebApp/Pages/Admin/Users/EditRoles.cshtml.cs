using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Staff;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.Roles;
using AirWeb.WebApp.Models;

namespace AirWeb.WebApp.Pages.Admin.Users;

[Authorize(Policy = nameof(Policies.UserAdministrator))]
public class EditRolesModel(IStaffService staffService) : PageModel
{
    [FromRoute]
    public Guid? Id { get; set; }

    [BindProperty]
    public List<RoleSetting> RoleSettings { get; set; } = [];

    public StaffViewDto DisplayStaff { get; private set; } = null!;
    public string? OfficeName => DisplayStaff.Office?.Name;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id is null) return RedirectToPage("Index");
        var staff = await staffService.FindAsync(Id.Value.ToString());
        if (staff is null) return NotFound();
        if (staff.Email is null) return BadRequest();

        DisplayStaff = staff;

        await PopulateRoleSettingsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Id is null) return BadRequest();
        var rolesDictionary = RoleSettings.ToDictionary(setting => setting.Name, setting => setting.IsSelected);
        var result = await staffService.UpdateRolesAsync(Id.Value.ToString(), rolesDictionary);

        if (result.Succeeded)
        {
            TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "User roles successfully updated.");
            return RedirectToPage("Details", new { id = Id });
        }

        foreach (var err in result.Errors)
            ModelState.AddModelError(string.Empty, string.Concat(err.Code, ": ", err.Description));

        var staff = await staffService.FindAsync(Id.Value.ToString());
        if (staff?.Email is null) return BadRequest();

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
