using AirWeb.AppServices.Compliance.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;
using AirWeb.AppServices.Compliance.Compliance.Fces.Search;
using AirWeb.AppServices.Compliance.Compliance.SourceTests;
using AirWeb.AppServices.Compliance.Enforcement.Search;
using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using IaipDataService.SourceTests.Models;

namespace AirWeb.WebApp.Pages.Facility;

[Authorize(Policy = nameof(Policies.Staff))]
public class DetailsModel(
    IFacilityService facilityService,
    IFceSearchService fceSearchService,
    ICaseFileSearchService caseFileService,
    IComplianceWorkSearchService searchService,
    ISourceTestAppService sourceTestService,
    IAuthorizationService authorization) : PageModel
{
    // Facility
    [FromRoute]
    public string? Id { get; set; }

    public IaipDataService.Facilities.Facility? Facility { get; private set; }

    // Data tables
    public IPaginatedResult<CaseFileSearchResultDto> CaseFiles { get; set; } = null!;
    public IPaginatedResult<ComplianceWorkSearchResultDto> ComplianceWork { get; set; } = null!;
    public IPaginatedResult<FceSearchResultDto> Fces { get; set; } = null!;

    public IPaginatedResult<SourceTestSummary> SourceTests { get; private set; } = null!;

    [TempData]
    public bool RefreshIaipData { get; set; }

    // Permissions
    public bool IsComplianceStaff { get; private set; }

    public async Task<IActionResult> OnGetAsync([FromQuery] bool refresh = false, CancellationToken token = default)
    {
        if (string.IsNullOrEmpty(Id)) return RedirectToPage("Index");

        if (refresh)
        {
            RefreshIaipData = true;
            return RedirectToPage();
        }

        if (!FacilityId.TryParse(Id, out var facilityId)) return NotFound("Facility ID not found.");

        if (facilityId.FormattedId != Id) return RedirectToPage(new { id = facilityId });

        Facility = await facilityService.FindFacilityDetailsAsync(facilityId, RefreshIaipData);
        if (Facility is null) return NotFound("Facility ID not found.");

        // Source Test service can be run in parallel with the search services.
        var sourceTestsForFacilityTask = sourceTestService.GetSourceTestsForFacilityAsync(facilityId,
            PaginationDefaults.SourceTestSummary, RefreshIaipData);

        // Search services cannot be run in parallel with each other when using Entity Framework.
        ComplianceWork = await searchService.SearchAsync(SearchDefaults.FacilityCompliance(Id),
            PaginationDefaults.ComplianceSummary, loadFacilities: false, token: token);

        Fces = await fceSearchService.SearchAsync(SearchDefaults.FacilityFces(Id),
            PaginationDefaults.FceSummary, loadFacilities: false, token: token);

        CaseFiles = await caseFileService.SearchAsync(SearchDefaults.FacilityEnforcement(Id),
            PaginationDefaults.EnforcementSummary, loadFacilities: false, token: token);

        SourceTests = await sourceTestsForFacilityTask;

        IsComplianceStaff = await authorization.Succeeded(User, CompliancePolicies.ComplianceStaff);
        return Page();
    }
}
