using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.AppServices.Lookups.Offices;
using AirWeb.AppServices.Staff;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.WebApp.Models;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Account;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class EditModel(IStaffService staffService, IOfficeService officeService, IValidator<StaffUpdateDto> validator)
    : PageModel, ISubmitCancelButtons
{
    [BindProperty]
    public StaffUpdateDto UpdateStaff { get; set; } = null!;

    public StaffViewDto DisplayStaff { get; private set; } = null!;

    public SelectList OfficeSelectList { get; private set; } = null!;

    // Form buttons
    public string SubmitText => "Update Info";
    public string CancelRoute => "Index";
    public string RouteId => string.Empty;

    public async Task<IActionResult> OnGetAsync()
    {
        DisplayStaff = await staffService.GetCurrentUserAsync();
        if (DisplayStaff.Email is null) return BadRequest();

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

        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "Successfully updated profile.");
        return RedirectToPage("Index");
    }

    private async Task PopulateSelectListsAsync() =>
        OfficeSelectList = (await officeService.GetAsListItemsAsync()).ToSelectList();
}
