using AirWeb.AppServices.AuthenticationServices.Claims;
using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.AppServices.Enforcement.Search;
using AirWeb.WebApp.Platform.Defaults;
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
    public bool ShowDashboard { get; private set; }
    public bool IsStaff { get; private set; }
    public bool IsComplianceStaff { get; private set; }
    public bool IsComplianceManager { get; private set; }
    public bool IsEnforcementReviewer { get; private set; }
    public string? UserId { get; set; }
    public Guid? OfficeId { get; set; }

    // Dashboard cards

    // -- Compliance staff
    public IPaginatedResult<WorkEntrySearchResultDto> ComplianceWork { get; private set; } = null!;
    public IList<SourceTestSummary> SourceTests { get; private set; } = [];
    public IPaginatedResult<CaseFileSearchResultDto> CaseFiles { get; private set; } = null!;

    // -- Compliance manager
    public IPaginatedResult<WorkEntrySearchResultDto> OfficeComplianceWork { get; private set; } = null!;
    public IList<SourceTestSummary> OfficeSourceTests { get; private set; } = [];

    // -- Enforcement reviewer/manager
    public IPaginatedResult<CaseFileSearchResultDto> EnforcementReviews { get; private set; } = null!;
    public IPaginatedResult<CaseFileSearchResultDto> OfficeCaseFiles { get; private set; } = null!;

    public async Task<IActionResult> OnGet(CancellationToken token = default)
    {
        if (User.Identity is not { IsAuthenticated: true })
            return Challenge();

        IsStaff = await authorization.Succeeded(User, Policies.Staff);
        if (!IsStaff) return Page();

        UserId = User.GetNameIdentifierId();
        if (UserId is null) return Page();

        ShowDashboard = true;
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
        await LoadSourceTests(token);
        await LoadCaseFiles(token);
    }

    private async Task LoadComplianceWork(CancellationToken token)
    {
        ComplianceWork = await complianceSearchService.SearchAsync(SearchDefaults.StaffOpenCompliance(UserId!),
            PaginationDefaults.ComplianceSummary, token: token);
    }

    private async Task LoadSourceTests(CancellationToken token)
    {
        // Requires an addition to the IAIP data service.
    }

    private async Task LoadCaseFiles(CancellationToken token)
    {
        CaseFiles = await caseFileSearchService.SearchAsync(SearchDefaults.StaffOpenEnforcement(UserId!),
            PaginationDefaults.EnforcementSummary, token: token);
    }

    // Load compliance manager tables
    private async Task LoadComplianceManagerTables(CancellationToken token)
    {
        if (OfficeId is null) return;
        await LoadOfficeComplianceWork(token);
        await LoadOfficeSourceTests(token);
    }

    private async Task LoadOfficeComplianceWork(CancellationToken token)
    {
        OfficeComplianceWork = await complianceSearchService.SearchAsync(
            SearchDefaults.OfficeOpenCompliance(OfficeId!.Value), PaginationDefaults.ComplianceSummary, token: token);
    }

    private async Task LoadOfficeSourceTests(CancellationToken token)
    {
        // Requires an addition to the IAIP data service.
    }

    // Load enforcement reviewer tables
    private async Task LoadEnforcementReviewerTables(CancellationToken token)
    {
        await LoadReviewRequests();
        if (OfficeId is null) return;
        await LoadOfficeEnforcementWork(token);
    }

    private async Task LoadReviewRequests()
    {
        // Load enforcement action review requests
        // Requires adding an enforcement action search
    }

    private async Task LoadOfficeEnforcementWork(CancellationToken token) =>
        OfficeCaseFiles = await caseFileSearchService.SearchAsync(SearchDefaults.OfficeOpenEnforcement(OfficeId!.Value),
            PaginationDefaults.EnforcementSummary, token: token);
}
