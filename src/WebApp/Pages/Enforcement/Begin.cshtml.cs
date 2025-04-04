using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.CaseFileCommand;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class BeginModel(
    IFacilityService facilityService,
    IWorkEntryService entryService,
    ICaseFileService caseFileService,
    IStaffService staffService,
    IValidator<CaseFileCreateDto> validator) : PageModel, ISubmitCancelButtons
{
    [FromRoute]
    public string? FacilityId { get; set; }

    [FromRoute]
    public int? EventId { get; set; }

    [BindProperty]
    public CaseFileCreateDto NewCaseFile { get; set; } = null!;

    public IaipDataService.Facilities.Facility? Facility { get; private set; }
    public IWorkEntrySummaryDto? ComplianceEvent { get; private set; }
    public SelectList StaffSelectList { get; private set; } = null!;
    private const string FacilityIdNotFound = "Facility not found.";

    // Form buttons
    // Cancel redirects either to Event ID if set or Facility ID
    public string SubmitText => "Begin Enforcement Case";
    public string CancelRoute => ComplianceEvent == null ? "/Facility/Details" : "/Compliance/Work/Details";
    public string RouteId => (ComplianceEvent == null ? FacilityId : EventId.ToString()) ?? string.Empty;

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        if (FacilityId == null || EventId == 0) return RedirectToPage("Index");

        Facility = await facilityService.FindFacilityDetailsAsync((FacilityId)FacilityId);
        if (Facility is null) return NotFound(FacilityIdNotFound);

        if (EventId != null)
        {
            ComplianceEvent = await entryService.FindAsync(EventId!.Value, includeComments: false, token);
            if (ComplianceEvent is null) return NotFound("Compliance event not found.");
            if (ComplianceEvent.FacilityId != FacilityId) return BadRequest();
            if (!User.CanBeginEnforcement(ComplianceEvent)) return Forbid();
        }

        NewCaseFile = new CaseFileCreateDto
        {
            FacilityId = FacilityId,
            EventId = EventId,
            ResponsibleStaffId = ComplianceEvent?.ResponsibleStaff?.Id ?? (await staffService.GetCurrentUserAsync()).Id,
            DiscoveryDate = ComplianceEvent?.EventDate ?? DateOnly.FromDateTime(DateTime.Today),
        };

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (NewCaseFile.FacilityId is null) return NotFound(FacilityIdNotFound);
        await validator.ApplyValidationAsync(NewCaseFile, ModelState);

        if (!ModelState.IsValid)
        {
            Facility = await facilityService.FindFacilitySummaryAsync((FacilityId)NewCaseFile.FacilityId);
            if (Facility is null) return BadRequest(FacilityIdNotFound);

            if (EventId != null)
            {
                ComplianceEvent = await entryService.FindAsync(EventId!.Value, includeComments: false, token);
                if (ComplianceEvent is null || ComplianceEvent.FacilityId != FacilityId ||
                    !User.CanBeginEnforcement(ComplianceEvent))
                    return BadRequest();
            }

            await PopulateSelectListsAsync();
            return Page();
        }

        var createResult = await caseFileService.CreateAsync(NewCaseFile, token);

        const string message = "Enforcement Case File successfully created.";
        if (createResult.HasAppNotificationFailure)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning, message,
                createResult.AppNotificationResult!.FailureMessage);
        }
        else
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, message);
        }

        return RedirectToPage("Details", new { createResult.Id });
    }

    private async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();
}
