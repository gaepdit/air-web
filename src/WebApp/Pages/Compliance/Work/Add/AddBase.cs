using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.WebApp.Models;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Compliance.Work.Add;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public abstract class AddBase(IFacilityService facilityService, IStaffService staffService)
    : PageModel, ISubmitCancelButtons
{
    [FromRoute]
    public string? FacilityId { get; set; }

    public WorkEntryType EntryType { get; protected set; }
    public IaipDataService.Facilities.Facility? Facility { get; protected set; }
    public SelectList StaffSelectList { get; private set; } = null!;

    // Form buttons
    public string SubmitText => $"Add {EntryType.GetDisplayName()}";
    public string CancelRoute => "/Facility/Details";
    public string RouteId => FacilityId ?? string.Empty;

    protected async Task<IActionResult> DoGetAsync()
    {
        if (FacilityId is null) return NotFound("Facility ID not found.");
        Facility = await facilityService.FindFacilityDetailsAsync((FacilityId)FacilityId);
        if (Facility is null) return NotFound("Facility ID not found.");

        await PopulateSelectListsAsync();
        return Page();
    }

    protected async Task<IActionResult> DoPostAsync<TDto>(
        TDto item, IWorkEntryService entryService,
        IValidator<TDto> validator, CancellationToken token)
        where TDto : IWorkEntryCreateDto
    {
        if (item.FacilityId == null || FacilityId != item.FacilityId) return BadRequest();
        await validator.ApplyValidationAsync(item, ModelState);

        if (!ModelState.IsValid)
        {
            Facility = await facilityService.FindFacilitySummaryAsync((FacilityId)item.FacilityId);
            if (Facility is null) return BadRequest();

            await PopulateSelectListsAsync();
            return Page();
        }

        var createResult = await entryService.CreateAsync(item, token);

        var message = $"{EntryType.GetDisplayName()} successfully created.";
        if (createResult.HasAppNotificationFailure)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning, message,
                createResult.AppNotificationResult!.FailureMessage);
        }
        else
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, message);
        }

        return RedirectToPage("../Details", new { createResult.Id });
    }

    protected virtual async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();
}
