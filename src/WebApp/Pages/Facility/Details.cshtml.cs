using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.Fces.Search;
using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.AppServices.Enforcement.Search;
using AirWeb.WebApp.Platform.Constants;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace AirWeb.WebApp.Pages.Facility;

[Authorize(Policy = nameof(Policies.Staff))]
public class DetailsModel(
    IFacilityService facilityService,
    IFceSearchService fceSearchService,
    ICaseFileSearchService caseFileService,
    IWorkEntrySearchService entrySearchService,
    ISourceTestService sourceTestService,
    IAuthorizationService authorization) : PageModel
{
    // Facility
    [FromRoute]
    public string? Id { get; set; }

    public IaipDataService.Facilities.Facility? Facility { get; private set; }

    // Data tables
    public IList<CaseFileSearchResultDto> EnforcementWork { get; private set; } = [];
    public int EnforcementCount { get; private set; }
    public IList<WorkEntrySearchResultDto> ComplianceWork { get; private set; } = [];
    public int ComplianceWorkCount { get; private set; }
    public IList<FceSearchResultDto> Fces { get; private set; } = [];
    public int FceCount { get; private set; }
    public IList<SourceTestSummary> SourceTests { get; private set; } = [];
    public int SourceTestCount { get; private set; }

    [TempData]
    public bool RefreshIaipData { get; set; }

    // Permissions
    public bool IsComplianceStaff { get; private set; }

    public async Task<IActionResult> OnGetAsync([FromQuery] bool refresh = false, CancellationToken token = default)
    {
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
        var sourceTestsForFacilityTask =
            sourceTestService.GetSourceTestsForFacilityAsync(facilityId, RefreshIaipData);

        // Search services cannot be run in parallel with each other when using Entity Framework.
        var searchWorkEntries = await entrySearchService.SearchAsync(
            new WorkEntrySearchDto { Sort = WorkEntrySortBy.EventDateDesc, PartialFacilityId = Id },
            new PaginatedRequest(1, GlobalConstants.SummaryTableSize),
            loadFacilities: false, token: token);

        var searchFces = await fceSearchService.SearchAsync(
            new FceSearchDto { Sort = FceSortBy.YearDesc, PartialFacilityId = Id },
            new PaginatedRequest(1, GlobalConstants.SummaryTableSize),
            loadFacilities: false, token: token);

        var searchEnforcement = await caseFileService.SearchAsync(
            new CaseFileSearchDto { Sort = CaseFileSortBy.DiscoveryDateAsc, PartialFacilityId = Id },
            new PaginatedRequest(1, GlobalConstants.SummaryTableSize),
            loadFacilities: false, token: token);

        ComplianceWork = searchWorkEntries.Items.ToList();
        ComplianceWorkCount = searchWorkEntries.TotalCount;

        Fces = searchFces.Items.ToList();
        FceCount = searchFces.TotalCount;

        EnforcementWork = searchEnforcement.Items.ToList();
        EnforcementCount = searchEnforcement.TotalCount;

        var sourceTestsForFacility = await sourceTestsForFacilityTask;
        SourceTests = sourceTestsForFacility.Take(GlobalConstants.SummaryTableSize).ToList();
        SourceTestCount = sourceTestsForFacility.Count;

        IsComplianceStaff = await authorization.Succeeded(User, Policies.ComplianceStaff);
        return Page();
    }
}
