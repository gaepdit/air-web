using AirWeb.AppServices.Compliance.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using AirWeb.AppServices.Compliance.Enforcement;
using AirWeb.AppServices.Compliance.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Compliance.Enforcement.Permissions;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.Domain.Compliance.AppRoles;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.WebApp.Models;
using GaEpd.AppLibrary.ListItems;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Compliance.Work.Add;

[Authorize(Policy = nameof(CompliancePolicies.ComplianceStaff))]
public abstract class AddBase(
    IFacilityService facilityService,
    IComplianceWorkService complianceService,
    ICaseFileService? caseFileService,
    IStaffService staffService) : PageModel, ISubmitCancelButtons
{
    [FromRoute]
    public string? FacilityId { get; set; }

    [FromQuery]
    public int? CaseFileId { get; set; }

    public ComplianceWorkType WorkType { get; protected set; }
    public IaipDataService.Facilities.Facility? Facility { get; protected set; }
    public CaseFileSummaryDto? CaseFile { get; protected set; }

    public SelectList StaffSelectList { get; private set; } = null!;

    // Form buttons
    public string SubmitText => $"Add {WorkType.GetDisplayName()}";
    public string CancelRoute { get; private set; } = "/Facility/Details";
    public string RouteId { get; private set; } = string.Empty;

    [TempData]
    public int HighlightId { get; set; }

    protected Task<IActionResult> DoGetAsync(CancellationToken token = default) => GeneratePage(token);

    protected async Task<IActionResult> DoPostAsync<TDto>(TDto item, IValidator<TDto> validator,
        CancellationToken token = default)
        where TDto : IComplianceWorkCreateDto
    {
        item.FacilityId = FacilityId;
        await validator.ApplyValidationAsync(item, ModelState);
        if (!ModelState.IsValid) return await GeneratePage(token);

        var result = await complianceService.CreateAsync(item, User, token);

        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success,
            $"{WorkType.GetDisplayName()} successfully created{(CaseFileId is null ? "" : " and linked")}.");
        if (result.HasWarning) TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, result.WarningMessage);

        if (CaseFileId is null) return RedirectToPage("../Details", new { result.Id });

        HighlightId = result.Id;
        return RedirectToPage("/Enforcement/LinkedEvents", new { id = CaseFileId });
    }

    private async Task<IActionResult> GeneratePage(CancellationToken token)
    {
        if (FacilityId is null && CaseFileId is null || (FacilityId is not null && CaseFileId is not null))
            return BadRequest();

        RouteId = FacilityId ?? CaseFileId.ToString() ?? string.Empty;

        if (CaseFileId is not null)
        {
            if (!WorkType.IsComplianceEventWorkType() || caseFileService is null) return BadRequest();
            CaseFile = await caseFileService.FindSummaryAsync(CaseFileId.Value, token);
            if (CaseFile is null) return BadRequest("Case File not found.");
            if (!User.CanEditCaseFile(CaseFile)) return Forbid();
            FacilityId = CaseFile.FacilityId;
            CancelRoute = "/Enforcement/LinkedEvents";
        }

        Facility = await facilityService.FindFacilityAsync((FacilityId)FacilityId!, token: token);
        if (Facility is null) return BadRequest();

        await PopulateSelectListsAsync(token);
        return Page();
    }

    protected virtual async Task PopulateSelectListsAsync(CancellationToken token) =>
        StaffSelectList = (await staffService.GetStaffInRoleAsync([
            ComplianceRole.ComplianceStaffRole,
            ComplianceRole.ComplianceManagerRole,
        ], token: token).ConfigureAwait(false)).ToSelectList();
}
