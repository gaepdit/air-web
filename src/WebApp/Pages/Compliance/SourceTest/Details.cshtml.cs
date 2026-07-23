using AirWeb.AppServices.Compliance.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.SourceTestReviews;
using AirWeb.AppServices.Compliance.Compliance.Permissions;
using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.AppServices.Core.EntityServices.Comments;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.Domain.Compliance.AppRoles;
using AirWeb.WebApp.Models;
using GaEpd.AppLibrary.ListItems;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.WebApp.Pages.Compliance.SourceTest;

[Authorize(Policy = nameof(Policies.Staff))]
public class DetailsModel(
    ISourceTestService testService,
    IComplianceWorkService compliance,
    IStaffService staff,
    IAuthorizationService authorization,
    IValidator<SourceTestReviewCreateDto> validator)
    : PageModel
{
    // Source test
    [FromRoute]
    public int ReferenceNumber { get; set; }

    public SourceTestSummary? TestSummary { get; private set; }

    // Compliance review
    public SourceTestReviewViewDto? ComplianceReview { get; private set; }
    public CommentsSectionModel? CommentSection { get; set; }
    public SourceTestReviewCreateDto? NewComplianceReview { get; set; }
    public SelectList? StaffSelectList { get; private set; }

    // Permissions

    // This `CanAddReview` bool is only used if a compliance review (`SourceTestReview`) does not exist.
    public bool CanAddNewReview { get; private set; }

    // This `UserCan` dictionary is only used if a compliance review (`SourceTestReview`) does exist.
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    [Display(Name = "Linked Enforcement Cases")]
    public IEnumerable<int> CaseFileIds { get; private set; } = [];

    [TempData]
    public Guid NewCommentId { get; set; }

    [TempData]
    public string? NotificationFailureMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken token = default)
    {
        if (ReferenceNumber == 0) return RedirectToPage("./Index");

        TestSummary = await testService.FindSummaryAsync(ReferenceNumber);
        if (TestSummary is null) return NotFound();

        ComplianceReview = await compliance.FindSourceTestReviewAsync(ReferenceNumber, token);
        await SetPermissionsAsync(token);

        if (ComplianceReview is null && !CanAddNewReview) return Page();

        if (ComplianceReview is not null)
        {
            CaseFileIds = await compliance.GetCaseFileIdsAsync(ComplianceReview.Id, token);

            CommentSection = new CommentsSectionModel
            {
                Comments = ComplianceReview.Comments,
                NewComment = new CommentAddDto(ComplianceReview.Id),
                NewCommentId = NewCommentId,
                NotificationFailureMessage = NotificationFailureMessage,
                CanAddComment = UserCan[ComplianceOperation.AddComment],
                CanDeleteComment = UserCan[ComplianceOperation.DeleteComment],
            };

            return Page();
        }

        var defaultStaff = await staff.FindByEmailAsync(TestSummary.IaipComplianceAssignment);

        var defaultStaffId =
            defaultStaff != null &&
            await staff.IsInRoleAsync(defaultStaff.Id, ComplianceRole.ComplianceStaffRole,
                ComplianceRole.ComplianceManagerRole)
                ? defaultStaff.Id
                : (await staff.GetCurrentUserAsync()).Id;

        var defaultReceivedDate = TestSummary.DateTestReviewComplete is null
            ? DateOnly.FromDateTime(DateTime.Today)
            : DateOnly.FromDateTime(TestSummary.DateTestReviewComplete.Value);

        NewComplianceReview = new SourceTestReviewCreateDto
        {
            ReferenceNumber = ReferenceNumber,
            TestReportIsClosed = TestSummary.ReportClosed,
            FacilityId = TestSummary.Facility?.FacilityId,
            ReceivedByComplianceDate = defaultReceivedDate,
            ResponsibleStaffId = defaultStaffId,
        };

        await PopulateSelectListsAsync(token);

        return Page();
    }

    private const string DetailsPageName = "Details";

    public async Task<IActionResult> OnPostNewReviewAsync(SourceTestReviewCreateDto newComplianceReview,
        CancellationToken token)
    {
        if (newComplianceReview.FacilityId == null || newComplianceReview.ReferenceNumber == 0 ||
            ReferenceNumber != newComplianceReview.ReferenceNumber)
            return BadRequest();

        TestSummary = await testService.FindSummaryAsync(ReferenceNumber);
        if (TestSummary is null) return BadRequest();

        await SetPermissionsAsync(token);
        if (!CanAddNewReview) return BadRequest();

        newComplianceReview.TestReportIsClosed = TestSummary.ReportClosed;
        await validator.ApplyValidationAsync(newComplianceReview, ModelState);

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync(token);
            return Page();
        }

        var result = await compliance.CreateAsync(newComplianceReview, token);

        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "Compliance Review successfully created.");
        if (result.HasWarning) TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, result.WarningMessage);
        return RedirectToPage(DetailsPageName, pageHandler: null, routeValues: new { ReferenceNumber },
            fragment: "compliance-review");
    }

    public async Task<IActionResult> OnPostCloseAsync(CancellationToken token)
    {
        if (ReferenceNumber == 0 || !ModelState.IsValid) return BadRequest();

        var id = (await compliance.FindSourceTestReviewAsync(ReferenceNumber, token))?.Id;
        if (id is null) return BadRequest();

        var item = await compliance.FindSummaryAsync(id.Value, token);
        if (item is null || !User.CanClose(item)) return BadRequest();

        var result = await compliance.CloseAsync(id.Value, token);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, $"The {item.ItemName} has been closed.");
        if (result.HasWarning) TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, result.WarningMessage);

        return RedirectToPage(DetailsPageName, pageHandler: null, routeValues: new { ReferenceNumber },
            fragment: "compliance-review");
    }

    public async Task<IActionResult> OnPostReopenAsync(CancellationToken token)
    {
        if (ReferenceNumber == 0 || !ModelState.IsValid) return BadRequest();

        var id = (await compliance.FindSourceTestReviewAsync(ReferenceNumber, token))?.Id;
        if (id is null) return BadRequest();

        var item = await compliance.FindSummaryAsync(id.Value, token);
        if (item is null || !User.CanReopen(item)) return BadRequest();

        var result = await compliance.ReopenAsync(id.Value, token);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, $"The {item.ItemName} has been reopened.");
        if (result.HasWarning) TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, result.WarningMessage);

        return RedirectToPage(DetailsPageName, pageHandler: null, routeValues: new { ReferenceNumber },
            fragment: "compliance-review");
    }

    public async Task<IActionResult> OnPostNewCommentAsync(CommentAddDto newComment,
        CancellationToken token)
    {
        TestSummary = await testService.FindSummaryAsync(ReferenceNumber);
        if (TestSummary is null) return BadRequest();

        ComplianceReview = await compliance.FindSourceTestReviewAsync(ReferenceNumber, token);
        if (ComplianceReview is null || ComplianceReview.IsDeleted) return BadRequest();

        await SetPermissionsAsync(token);
        if (!UserCan[ComplianceOperation.AddComment]) return BadRequest();

        if (!ModelState.IsValid)
        {
            CaseFileIds = await compliance.GetCaseFileIdsAsync(ComplianceReview.Id, token);

            CommentSection = new CommentsSectionModel
            {
                Comments = ComplianceReview.Comments,
                NewComment = newComment,
                CanAddComment = UserCan[ComplianceOperation.AddComment],
                CanDeleteComment = UserCan[ComplianceOperation.DeleteComment],
            };

            return Page();
        }

        var result = await compliance.AddCommentAsync(ComplianceReview.Id, newComment, token);
        NewCommentId = result.Id;
        if (result.HasWarning) TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, result.WarningMessage);
        return RedirectToPage(DetailsPageName, pageHandler: null, routeValues: new { ReferenceNumber },
            fragment: NewCommentId.ToString());
    }

    public async Task<IActionResult> OnPostDeleteCommentAsync(Guid commentId, CancellationToken token)
    {
        TestSummary = await testService.FindSummaryAsync(ReferenceNumber);
        if (TestSummary is null) return BadRequest();

        ComplianceReview = await compliance.FindSourceTestReviewAsync(ReferenceNumber, token);
        if (ComplianceReview is null || ComplianceReview.IsDeleted) return BadRequest();

        await SetPermissionsAsync(token);
        if (!UserCan[ComplianceOperation.DeleteComment]) return BadRequest();

        await compliance.DeleteCommentAsync(commentId, token);
        return RedirectToPage(DetailsPageName, pageHandler: null, routeValues: new { ReferenceNumber },
            fragment: "comments");
    }

    private async Task SetPermissionsAsync(CancellationToken token)
    {
        // FUTURE: Refactor this permission check.
        CanAddNewReview = ComplianceReview is null &&
                          await authorization.Succeeded(User, CompliancePolicies.ComplianceStaff) &&
                          TestSummary is { ReportClosed: true } &&
                          !await compliance.SourceTestReviewExistsAsync(ReferenceNumber, token);
        if (ComplianceReview is not null)
            UserCan = await authorization.SetPermissions(ComplianceOperation.AllOperations, User, ComplianceReview);
    }

    private async Task PopulateSelectListsAsync(CancellationToken token) =>
        StaffSelectList = (await staff.GetStaffInRoleAsync(token, ComplianceRole.ComplianceStaffRole,
            ComplianceRole.ComplianceManagerRole).ConfigureAwait(false)).ToSelectList();
}
