using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using AirWeb.AppServices.Offices;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;

namespace AirWeb.WebApp.Pages.Admin.Users;

[Authorize(Policy = nameof(Policies.UserAdministrator))]
public class EditModel(IStaffService staffService, IOfficeService officeService, IValidator<StaffUpdateDto> validator)
    : PageModel
{
    [FromRoute]
    public Guid Id { get; set; }

    [BindProperty]
    public StaffUpdateDto Item { get; set; } = default!;

    public StaffViewDto DisplayStaff { get; private set; } = default!;

    public SelectList OfficesSelectList { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (id is null) return RedirectToPage("Index");
        if (!Guid.TryParse(id, out var guid)) return NotFound();

        var staff = await staffService.FindAsync(id);
        if (staff is null) return NotFound();

        Id = guid;
        DisplayStaff = staff;
        Item = DisplayStaff.AsUpdateDto();

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            var staff = await staffService.FindAsync(Id.ToString());
            if (staff is null) return BadRequest();

            DisplayStaff = staff;

            await PopulateSelectListsAsync();
            return Page();
        }

        var result = await staffService.UpdateAsync(Id.ToString(), Item);
        if (!result.Succeeded) return BadRequest();

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Successfully updated.");
        return RedirectToPage("Details", new { Id });
    }

    private async Task PopulateSelectListsAsync() =>
        OfficesSelectList = (await officeService.GetAsListItemsAsync()).ToSelectList();
}
