using AirWeb.AppServices.AuthenticationServices.Claims;
using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.CommonSearch;
using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.AppServices.Enforcement.Search;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.SourceTests.Models;
using Microsoft.Identity.Web;

namespace AirWeb.WebApp.Pages;

[AllowAnonymous]
public class IndexModel(
    IWorkEntrySearchService complianceSearchService,
    ICaseFileSearchService caseFileSearchService,
    IAuthorizationService authorization) : PageModel
{
    public bool IsStaff { get; private set; }
    public bool IsComplianceStaff { get; private set; }
    public bool IsComplianceManager { get; private set; }
    public bool IsEnforcementReviewer { get; private set; }
    private string? UserId { get; set; }
    private Guid? OfficeId { get; set; }

    // Dashboard cards

    // -- Compliance staff
    public IList<WorkEntrySearchResultDto> ComplianceWork { get; private set; } = [];
    public IList<SourceTestSummary> SourceTests { get; private set; } = [];
    public IList<CaseFileSearchResultDto> CaseFiles { get; private set; } = [];

    // -- Compliance manager
    public IList<WorkEntrySearchResultDto> OfficeComplianceWork { get; private set; } = [];
    public IList<SourceTestSummary> OfficeSourceTests { get; private set; } = [];

    // -- Enforcement reviewer/manager
    public IList<CaseFileSearchResultDto> EnforcementReviews { get; private set; } = [];
    public IList<CaseFileSearchResultDto> OfficeCaseFiles { get; private set; } = [];

    public async Task<IActionResult> OnGet(CancellationToken token = default)
    {
        if (User.Identity is not { IsAuthenticated: true })
            return Challenge();

        IsStaff = await authorization.Succeeded(User, Policies.Staff);
        if (!IsStaff) return Page();

        UserId = User.GetNameIdentifierId();
        if (UserId is null) return Page();
        OfficeId = User.GetOfficeId();

        IsComplianceStaff = await authorization.Succeeded(User, Policies.ComplianceStaff);
        if (IsComplianceStaff) await LoadComplianceStaffTables(token);

        IsComplianceManager = await authorization.Succeeded(User, Policies.ComplianceManager);
        if (IsComplianceManager) await LoadComplianceManagerTables(token);

        IsEnforcementReviewer = await authorization.Succeeded(User, Policies.EnforcementReviewer);
        if (IsEnforcementReviewer) await LoadEnforcementReviewerTables(token);

        return Page();
    }

    // Load compliance staff tables
    private async Task LoadComplianceStaffTables(CancellationToken token)
    {
        await LoadComplianceWork(token);
        // await LoadSourceTests(token);
        await LoadCaseFiles(token);
    }

    private async Task LoadComplianceWork(CancellationToken token)
    {
        var spec = new WorkEntrySearchDto { Closed = ClosedOpenAny.Open, Staff = UserId };
        var paging = new PaginatedRequest(1, 20, WorkEntrySortBy.EventDateDesc.GetDescription());
        ComplianceWork = (await complianceSearchService.SearchAsync(spec, paging, token: token)).Items.ToList();
    }

    private async Task LoadSourceTests(CancellationToken token)
    {
        // Requires an addition to the IAIP data service.
        throw new NotImplementedException();
    }

    private async Task LoadCaseFiles(CancellationToken token)
    {
        var spec = new CaseFileSearchDto { Closed = ClosedOpenAny.Open, Staff = UserId };
        var paging = new PaginatedRequest(1, 20, CaseFileSortBy.DiscoveryDateDesc.GetDescription());
        CaseFiles = (await caseFileSearchService.SearchAsync(spec, paging, token: token)).Items.ToList();
    }

    // Load compliance manager tables
    private async Task LoadComplianceManagerTables(CancellationToken token)
    {
        await LoadOfficeComplianceWork(token);
        // await LoadOfficeSourceTests(token);
    }

    private async Task LoadOfficeComplianceWork(CancellationToken token)
    {
        if (OfficeId is null) return;
        var spec = new WorkEntrySearchDto { Closed = ClosedOpenAny.Open, Office = OfficeId };
        var paging = new PaginatedRequest(1, 20, WorkEntrySortBy.EventDateDesc.GetDescription());
        OfficeComplianceWork = (await complianceSearchService.SearchAsync(spec, paging, token: token)).Items.ToList();
    }

    private async Task LoadOfficeSourceTests(CancellationToken token)
    {
        if (OfficeId is null) return;
        // Requires an addition to the IAIP data service.
        throw new NotImplementedException();
    }

    // Load enforcement reviewer tables
    private async Task LoadEnforcementReviewerTables(CancellationToken token)
    {
        // await LoadReviewRequests();
        await LoadOfficeEnforcementWork(token);
    }

    private async Task LoadReviewRequests()
    {
        // Load enforcement action review requests
        // Requires building an enforcement action search service
        throw new NotImplementedException();
    }

    private async Task LoadOfficeEnforcementWork(CancellationToken token)
    {
        if (OfficeId is null) return;
        var spec = new CaseFileSearchDto { Closed = ClosedOpenAny.Open, Office = OfficeId };
        var paging = new PaginatedRequest(1, 20, CaseFileSortBy.DiscoveryDateDesc.GetDescription());
        OfficeCaseFiles = (await caseFileSearchService.SearchAsync(spec, paging, token: token)).Items.ToList();
    }
}
