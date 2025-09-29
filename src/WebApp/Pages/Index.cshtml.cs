using AirWeb.AppServices.AuthenticationServices.Claims;
using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.SourceTests;
using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.EnforcementActionQuery;
using AirWeb.AppServices.Enforcement.Search;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Defaults;
using GaEpd.AppLibrary.Pagination;
using Microsoft.Identity.Web;

namespace AirWeb.WebApp.Pages;

[AllowAnonymous]
public class IndexModel(
    IWorkEntrySearchService complianceSearchService,
    ICaseFileSearchService caseFileSearchService,
    IEnforcementActionService enforcementActionService,
    ISourceTestAppService sourceTestService,
    IAuthorizationService authorization) : PageModel
{
    public bool ShowDashboard { get; private set; }
    public bool IsStaff { get; private set; }
    public bool IsComplianceStaff { get; private set; }
    public bool IsComplianceManager { get; private set; }
    public bool IsEnforcementReviewer { get; private set; }
    public string? UserId { get; set; }
    public string? UserEmail { get; set; }
    public Guid? UserOfficeId { get; set; }

    // Dashboard cards

    // -- Compliance staff
    public IPaginatedResult<WorkEntrySearchResultDto> StaffComplianceWork { get; private set; } = null!;
    public IPaginatedResult StaffSourceTests { get; private set; } = null!;
    public IPaginatedResult<CaseFileSearchResultDto> StaffCaseFiles { get; private set; } = null!;

    // -- Compliance manager
    public IPaginatedResult<WorkEntrySearchResultDto> OfficeComplianceWork { get; private set; } = null!;
    public IPaginatedResult OfficeSourceTests { get; private set; } = null!;

    // -- Enforcement reviewer/manager
    public IPaginatedResult<ActionViewDto> EnforcementReviews { get; private set; } = null!;
    public IPaginatedResult<CaseFileSearchResultDto> OfficeCaseFiles { get; private set; } = null!;

    public async Task<IActionResult> OnGet(CancellationToken token = default)
    {
        if (User.Identity is not { IsAuthenticated: true })
            return Challenge();

        IsStaff = await authorization.Succeeded(User, Policies.Staff);
        if (!IsStaff) return Page();

        UserId = User.GetNameIdentifierId();
        if (UserId is null) return Page();

        UserEmail = User.GetEmail();
        if (UserEmail is null) return Page();

        UserOfficeId = User.GetOfficeId();
        if (UserOfficeId is null)
        {
            TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, message: "Set your Office to proceed.");
            return RedirectToPage("Account/Edit");
        }

        ShowDashboard = true;

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
        await LoadStaffComplianceWork(token);
        await LoadStaffSourceTests();
        await LoadStaffCaseFiles(token);
    }

    private async Task LoadStaffComplianceWork(CancellationToken token) => StaffComplianceWork =
        await complianceSearchService.SearchAsync(SearchDefaults.StaffOpenCompliance(UserId!),
            PaginationDefaults.ComplianceSummary, token: token);

    private async Task LoadStaffSourceTests() => StaffSourceTests = await sourceTestService
        .GetOpenSourceTestsForComplianceAsync(UserEmail, PaginationDefaults.SourceTestSummary);

    private async Task LoadStaffCaseFiles(CancellationToken token) => StaffCaseFiles = await caseFileSearchService
        .SearchAsync(SearchDefaults.StaffOpenEnforcement(UserId!), PaginationDefaults.EnforcementSummary, token: token);

    // Load compliance manager tables
    private async Task LoadComplianceManagerTables(CancellationToken token)
    {
        await LoadOfficeComplianceWork(token);
        await LoadOfficeSourceTests();
    }

    private async Task LoadOfficeComplianceWork(CancellationToken token)
    {
        if (UserOfficeId is null) return;
        OfficeComplianceWork = await complianceSearchService.SearchAsync(
            SearchDefaults.OfficeOpenCompliance(UserOfficeId!.Value), PaginationDefaults.ComplianceSummary,
            token: token);
    }

    // FUTURE: This shows all open source tests, not just those limited to the user's office.
    private async Task LoadOfficeSourceTests()
    {
        if (UserOfficeId is null) return;
        OfficeSourceTests = await sourceTestService.GetOpenSourceTestsForComplianceAsync(userEmail: null,
            PaginationDefaults.SourceTestSummary);
    }

    // Load enforcement reviewer tables
    private async Task LoadEnforcementReviewerTables(CancellationToken token)
    {
        await LoadReviewRequests(token);
        await LoadOfficeEnforcementWork(token);
    }

    // FUTURE: This shows all assigned review requests without pagination or a link to a wider search page.
    private async Task LoadReviewRequests(CancellationToken token)
    {
        if (UserId is null) return;
        EnforcementReviews = await enforcementActionService
            .GetReviewRequestsAsync(UserId, PaginationDefaults.EnforcementBulk, token);
    }

    private async Task LoadOfficeEnforcementWork(CancellationToken token)
    {
        if (UserOfficeId is null) return;
        OfficeCaseFiles = await caseFileSearchService.SearchAsync(SearchDefaults
            .OfficeOpenEnforcement(UserOfficeId!.Value), PaginationDefaults.EnforcementSummary, token: token);
    }
}
