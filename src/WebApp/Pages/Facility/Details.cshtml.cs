using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.WebApp.Platform.Constants;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace AirWeb.WebApp.Pages.Facility;

[Authorize(Policy = nameof(Policies.Staff))]
public class DetailsModel(
    IFacilityService facilityService,
    IComplianceSearchService searchService,
    ISourceTestService sourceTestService,
    IAuthorizationService authorization) : PageModel
{
    // Facility
    [FromRoute]
    public string? FacilityId { get; set; }

    public IaipDataService.Facilities.Facility? Facility { get; private set; }

    // Data tables
    public IList<WorkEntrySearchResultDto> ComplianceWork { get; private set; } = [];
    public int ComplianceWorkCount { get; private set; }
    public IList<FceSearchResultDto> Fces { get; private set; } = [];
    public int FceCount { get; private set; }
    public IList<SourceTestSummary> SourceTests { get; private set; } = [];
    public int SourceTestCount { get; private set; }

    // Permissions
    public bool IsComplianceStaff { get; private set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken token = default)
    {
        if (FacilityId is null) return NotFound("Facility ID not found.");
        Facility = await facilityService.FindAsync((FacilityId)FacilityId);
        if (Facility is null) return NotFound("Facility ID not found.");

        // Source Test service can be run in parallel with the search service.
        var sourceTestsForFacilityTask = sourceTestService.GetSourceTestsForFacilityAsync((FacilityId)FacilityId);

        // Search service cannot be run in parallel with itself when using Entity Framework.
        var searchWorkEntries = await searchService.SearchWorkEntriesAsync(
            new WorkEntrySearchDto { Sort = SortBy.EventDateDesc, PartialFacilityId = FacilityId },
            new PaginatedRequest(1, GlobalConstants.SummaryTableSize),
            loadFacilities: false, token: token);

        var searchFces = await searchService.SearchFcesAsync(
            new FceSearchDto { Sort = SortBy.EventDateDesc, PartialFacilityId = FacilityId },
            new PaginatedRequest(1, GlobalConstants.SummaryTableSize),
            loadFacilities: false, token: token);

        ComplianceWork = searchWorkEntries.Items.ToList();
        ComplianceWorkCount = searchWorkEntries.TotalCount;

        Fces = searchFces.Items.ToList();
        FceCount = searchFces.TotalCount;

        var sourceTestsForFacility = await sourceTestsForFacilityTask;
        SourceTests = sourceTestsForFacility.Take(GlobalConstants.SummaryTableSize).ToList();
        SourceTestCount = sourceTestsForFacility.Count;

        IsComplianceStaff = await authorization.Succeeded(User, Policies.ComplianceStaff);
        return Page();
    }
}
