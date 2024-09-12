using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.Inspections;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Compliance.Work.Inspection;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class EditModel(
    IWorkEntryService entryService,
    IStaffService staffService,
    IAuthorizationService authorization) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty]
    public InspectionUpdateDto Item { get; set; } = default!;

    public WorkEntrySummaryDto ItemView { get; private set; } = default!;
    public WorkEntryType EntryType { get; protected set; }

    public SelectList StaffSelectList { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("../Index");

        var item = (InspectionUpdateDto?)await entryService.FindForUpdateAsync(Id);
        if (item is null) return NotFound();
        if (!await UserCanEditAsync(item)) return Forbid();

        var itemView = await entryService.FindSummaryAsync(Id);
        if (itemView is null) return BadRequest();

        Item = item;
        ItemView = itemView;
        EntryType = ItemView.WorkEntryType;

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var original = (InspectionUpdateDto?)await entryService.FindForUpdateAsync(Id, token);
        if (original is null || !await UserCanEditAsync(original)) return BadRequest();

        // TODO:
        // await validator.ApplyValidationAsync(item, ModelState);

        if (!ModelState.IsValid)
        {
            var itemView = await entryService.FindSummaryAsync(Id, token);
            if (itemView is null) return BadRequest();
            ItemView = itemView;
            EntryType = ItemView.WorkEntryType;

            await PopulateSelectListsAsync();
            return Page();
        }

        await entryService.UpdateAsync(Id, Item, token);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success,
            $"{EntryType.GetDescription()} successfully updated.");
        return RedirectToPage("../Details", new { Id });
    }

    // FUTURE: Allow for editing a Work Entry previously reviewed by a currently inactive user.
    private async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();

    private Task<bool> UserCanEditAsync(InspectionUpdateDto item) =>
        authorization.Succeeded(User, item, new InspectionUpdateRequirement());
}
