using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Compliance.Work.WorkEntryBase;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public abstract class EditBase(IWorkEntryService entryService, IStaffService staffService) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    public WorkEntrySummaryDto ItemView { get; protected set; } = null!;
    public SelectList StaffSelectList { get; private set; } = null!;

    protected async Task<IActionResult> DoGetAsync()
    {
        var itemView = await entryService.FindSummaryAsync(Id);
        if (itemView is null) return BadRequest();
        ItemView = itemView;

        await PopulateSelectListsAsync();
        return Page();
    }

    protected async Task<IActionResult> DoPostAsync<TDto>(
        TDto item, IValidator<TDto> validator, CancellationToken token)
        where TDto : IWorkEntryCommandDto
    {
        await validator.ApplyValidationAsync(item, ModelState);

        if (!ModelState.IsValid)
        {
            var itemView = await entryService.FindSummaryAsync(Id, token);
            if (itemView is null) return BadRequest();
            ItemView = itemView;

            await PopulateSelectListsAsync();
            return Page();
        }

        var notificationResult = await entryService.UpdateAsync(Id, item, token);
        var entryType = await entryService.GetWorkEntryTypeAsync(Id, token);
        TempData.SetDisplayMessage(
            notificationResult.Success ? DisplayMessage.AlertContext.Success : DisplayMessage.AlertContext.Warning,
            $"{entryType!.Value.GetDescription()} successfully updated.", notificationResult.FailureMessage);

        return RedirectToPage("../Details", new { Id });
    }

    // FUTURE: Allow for editing a Work Entry previously reviewed by a currently inactive user.
    protected virtual async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();
}
