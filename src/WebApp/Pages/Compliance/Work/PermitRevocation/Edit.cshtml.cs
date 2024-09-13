using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Compliance.Work.PermitRevocation;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class EditModel(
    IWorkEntryService entryService,
    IStaffService staffService,
    IAuthorizationService authorization,
    IValidator<PermitRevocationUpdateDto> validator) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty]
    public PermitRevocationUpdateDto Item { get; set; } = default!;

    public WorkEntrySummaryDto ItemView { get; private set; } = default!;
    public SelectList StaffSelectList { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("../Index");

        var item = (PermitRevocationUpdateDto?)await entryService.FindForUpdateAsync(Id);
        if (item is null) return NotFound();
        if (!await UserCanEditAsync(item)) return Forbid();

        var itemView = await entryService.FindSummaryAsync(Id);
        if (itemView is null) return BadRequest();

        Item = item;
        ItemView = itemView;

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var original = (PermitRevocationUpdateDto?)await entryService.FindForUpdateAsync(Id, token);
        if (original is null || !await UserCanEditAsync(original)) return BadRequest();

        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            var itemView = await entryService.FindSummaryAsync(Id, token);
            if (itemView is null) return BadRequest();
            ItemView = itemView;

            await PopulateSelectListsAsync();
            return Page();
        }

        var notificationResult = await entryService.UpdateAsync(Id, Item, token);
        const WorkEntryType entryType = WorkEntryType.PermitRevocation;
        TempData.SetDisplayMessage(
            notificationResult.Success ? DisplayMessage.AlertContext.Success : DisplayMessage.AlertContext.Warning,
            $"{entryType.GetDescription()} successfully updated.", notificationResult.FailureMessage);

        return RedirectToPage("../Details", new { Id });
    }

    // FUTURE: Allow for editing a Work Entry previously reviewed by a currently inactive user.
    private async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();

    private Task<bool> UserCanEditAsync(PermitRevocationUpdateDto item) =>
        authorization.Succeeded(User, item, new PermitRevocationUpdateRequirement());
}
