using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using AirWeb.AppServices.Offices;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;

namespace AirWeb.WebApp.Pages.Account;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class EditModel(IStaffService staffService, IOfficeService officeService, IValidator<StaffUpdateDto> validator)
    : PageModel
{
    [BindProperty]
    public StaffUpdateDto UpdateStaff { get; set; } = default!;

    public StaffViewDto DisplayStaff { get; private set; } = default!;

    public SelectList OfficeSelectList { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        DisplayStaff = await staffService.GetCurrentUserAsync();
        UpdateStaff = DisplayStaff.AsUpdateDto();

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var staff = await staffService.GetCurrentUserAsync();

        // User cannot deactivate self.
        UpdateStaff.Active = true;

        await validator.ApplyValidationAsync(UpdateStaff, ModelState);

        if (!ModelState.IsValid)
        {
            DisplayStaff = staff;
            await PopulateSelectListsAsync();
            return Page();
        }

        var result = await staffService.UpdateAsync(staff.Id, UpdateStaff);
        if (!result.Succeeded) return BadRequest();

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Successfully updated profile.");
        return RedirectToPage("Index");
    }

    private async Task PopulateSelectListsAsync() =>
        OfficeSelectList = (await officeService.GetAsListItemsAsync()).ToSelectList();
}
