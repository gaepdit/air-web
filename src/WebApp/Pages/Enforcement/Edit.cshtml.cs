﻿using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.Command;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.AppServices.Enforcement.Query;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class EditModel(
    IEnforcementService enforcementService,
    IStaffService staffService,
    IValidator<CaseFileUpdateDto> validator) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty]
    public CaseFileUpdateDto Item { get; set; } = null!;

    public CaseFileSummaryDto ItemView { get; private set; } = null!;
    public SelectList StaffSelectList { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        if (Id == 0) return RedirectToPage("Index");

        var itemView = await enforcementService.FindCaseFileSummaryAsync(Id, token);
        if (itemView is null) return NotFound();
        if (!User.CanEdit(itemView)) return Forbid();

        ItemView = itemView;
        Item = new CaseFileUpdateDto(ItemView);

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var itemView = await enforcementService.FindCaseFileSummaryAsync(Id, token);
        if (itemView is null || !User.CanEdit(itemView)) return BadRequest();
        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            ItemView = itemView;
            await PopulateSelectListsAsync();
            return Page();
        }

        await enforcementService.UpdateCaseFileAsync(Id, Item, token);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Enforcement successfully updated.");
        return RedirectToPage("Details", new { Id });
    }

    private async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();
}
