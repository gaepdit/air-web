using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.ComplianceStaff;
using AirWeb.AppServices.Staff;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using AutoMapper;
using FluentValidation;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Compliance.Work.Edit;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public abstract class EditBase(
    IWorkEntryService entryService,
    IStaffService staffService,
    IMapper mapper) : PageModel
{
    protected readonly IWorkEntryService EntryService = entryService;
    protected readonly IStaffService StaffService = staffService;
    protected readonly IMapper Mapper = mapper;

    [FromRoute]
    public int Id { get; set; }

    public IWorkEntrySummaryDto ItemView { get; protected set; } = null!;
    public SelectList StaffSelectList { get; private set; } = null!;

    protected async Task<IActionResult> DoGetAsync(CancellationToken token)
    {
        if (Id == 0) return RedirectToPage("../Index");

        var itemView = await EntryService.FindAsync(Id, false, token);
        if (itemView is null) return NotFound();
        if (!User.CanEdit(itemView)) return Forbid();

        ItemView = itemView;

        await PopulateSelectListsAsync();
        return Page();
    }

    protected async Task<IActionResult> DoPostAsync<TDto>(
        TDto item, IValidator<TDto> validator, CancellationToken token)
        where TDto : IWorkEntryCommandDto
    {
        var itemView = await EntryService.FindSummaryAsync(Id, token);
        if (itemView is null || !User.CanEdit(itemView)) return BadRequest();
        await validator.ApplyValidationAsync(item, ModelState);

        if (!ModelState.IsValid)
        {
            ItemView = itemView;
            await PopulateSelectListsAsync();
            return Page();
        }

        var notificationResult = await EntryService.UpdateAsync(Id, item, token);
        var entryType = await EntryService.GetWorkEntryTypeAsync(Id, token);
        var alertContext = notificationResult.Success
            ? DisplayMessage.AlertContext.Success
            : DisplayMessage.AlertContext.Warning;
        TempData.SetDisplayMessage(alertContext, $"{entryType!.Value.GetDescription()} successfully updated.",
            notificationResult.FailureMessage);

        return RedirectToPage("../Details", new { Id });
    }

    // FUTURE: Allow for editing a Work Entry previously reviewed by a currently inactive user.
    protected virtual async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await StaffService.GetAsListItemsAsync()).ToSelectList();
}
