﻿using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.NamedEntities.Offices;
using AirWeb.AppServices.Staff;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.WebApp.Models;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Admin.Users;

[Authorize(Policy = nameof(Policies.UserAdministrator))]
public class EditModel(IStaffService staffService, IOfficeService officeService, IValidator<StaffUpdateDto> validator)
    : PageModel, ISubmitCancelButtons
{
    [FromRoute]
    public Guid Id { get; set; }

    [BindProperty]
    public StaffUpdateDto Item { get; set; } = null!;

    public StaffViewDto DisplayStaff { get; private set; } = null!;

    public SelectList OfficesSelectList { get; private set; } = null!;

    // Form buttons
    public string SubmitText => "Update Info";
    public string CancelRoute => "Details";
    public string RouteId => Id.ToString();

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

        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "Successfully updated.");
        return RedirectToPage("Details", new { Id });
    }

    private async Task PopulateSelectListsAsync() =>
        OfficesSelectList = (await officeService.GetAsListItemsAsync()).ToSelectList();
}
