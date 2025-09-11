using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.Identity;
using AirWeb.WebApp.Models;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.WebApp.Pages.Compliance.SourceTest;

[Authorize(Policy = nameof(Policies.Staff))]
public class DetailsModel(
    ISourceTestService testService,
    IWorkEntryService entryService,
    IStaffService staffService,
    IAuthorizationService authorization,
    IValidator<SourceTestReviewCreateDto> validator)
    : PageModel
{
    // Source test
    [FromRoute]
    public int ReferenceNumber { get; set; }

    public SourceTestSummary? TestSummary { get; private set; }

    [TempData]
    public bool RefreshIaipData { get; set; }

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

    public async Task<IActionResult> OnGetAsync([FromQuery] bool refresh = false, CancellationToken token = default)
    {
        if (ReferenceNumber == 0) return RedirectToPage("./Index");

        if (refresh)
        {
            RefreshIaipData = true;
            return RedirectToPage();
        }

        TestSummary = await testService.FindSummaryAsync(ReferenceNumber, RefreshIaipData);
        if (TestSummary is null) return NotFound();

        ComplianceReview = await entryService.FindSourceTestReviewAsync(ReferenceNumber, token);
        await SetPermissionsAsync();

        if (ComplianceReview is null)
        {
            var defaultStaff = await staffService.FindByEmailAsync(TestSummary.IaipComplianceAssignment);

            var defaultStaffId =
                defaultStaff != null && await staffService.IsInRoleAsync(defaultStaff.Id, AppRole.ComplianceStaffRole)
                    ? defaultStaff.Id
                    : (await staffService.GetCurrentUserAsync()).Id;

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

            await PopulateSelectListsAsync();
        }
        else
        {
            CaseFileIds = await entryService.GetCaseFileIdsAsync(ComplianceReview.Id, token);

            CommentSection = new CommentsSectionModel
            {
                Comments = ComplianceReview.Comments,
                NewComment = new CommentAddDto(ComplianceReview.Id),
                NewCommentId = NewCommentId,
                NotificationFailureMessage = NotificationFailureMessage,
                CanAddComment = UserCan[ComplianceOperation.AddComment],
                CanDeleteComment = UserCan[ComplianceOperation.DeleteComment],
            };
        }

        return Page();
    }

    public async Task<IActionResult> OnPostNewReviewAsync(SourceTestReviewCreateDto newComplianceReview,
        CancellationToken token)
    {
        if (newComplianceReview.FacilityId == null || newComplianceReview.ReferenceNumber == 0 ||
            ReferenceNumber != newComplianceReview.ReferenceNumber)
            return BadRequest();

        TestSummary = await testService.FindSummaryAsync(ReferenceNumber);
        if (TestSummary is null) return BadRequest();

        await SetPermissionsAsync();
        if (!CanAddNewReview) return BadRequest();

        newComplianceReview.TestReportIsClosed = TestSummary.ReportClosed;
        await validator.ApplyValidationAsync(newComplianceReview, ModelState);

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        var result = await entryService.CreateAsync(newComplianceReview, token);

        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "Compliance Review successfully created.");
        if (result.HasWarning) TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, result.WarningMessage);
        return RedirectToPage("Index", pageHandler: null, routeValues: new { ReferenceNumber },
            fragment: "compliance-review");
    }

    public async Task<IActionResult> OnPostNewCommentAsync(CommentAddDto newComment,
        CancellationToken token)
    {
        TestSummary = await testService.FindSummaryAsync(ReferenceNumber);
        if (TestSummary is null) return BadRequest();

        ComplianceReview = await entryService.FindSourceTestReviewAsync(ReferenceNumber, token);
        if (ComplianceReview is null || ComplianceReview.IsDeleted) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[ComplianceOperation.AddComment]) return BadRequest();

        if (!ModelState.IsValid)
        {
            CaseFileIds = await entryService.GetCaseFileIdsAsync(ComplianceReview.Id, token);

            CommentSection = new CommentsSectionModel
            {
                Comments = ComplianceReview.Comments,
                NewComment = newComment,
                CanAddComment = UserCan[ComplianceOperation.AddComment],
                CanDeleteComment = UserCan[ComplianceOperation.DeleteComment],
            };

            return Page();
        }

        var result = await entryService.AddCommentAsync(ComplianceReview.Id, newComment, token);
        NewCommentId = result.Id;
        if (result.HasWarning) TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, result.WarningMessage);
        return RedirectToPage("Index", pageHandler: null, routeValues: new { ReferenceNumber },
            fragment: NewCommentId.ToString());
    }

    public async Task<IActionResult> OnPostDeleteCommentAsync(Guid commentId, CancellationToken token)
    {
        TestSummary = await testService.FindSummaryAsync(ReferenceNumber);
        if (TestSummary is null) return BadRequest();

        ComplianceReview = await entryService.FindSourceTestReviewAsync(ReferenceNumber, token);
        if (ComplianceReview is null || ComplianceReview.IsDeleted) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[ComplianceOperation.DeleteComment]) return BadRequest();

        await entryService.DeleteCommentAsync(commentId, token);
        return RedirectToPage("Index", pageHandler: null, routeValues: new { ReferenceNumber },
            fragment: "comments");
    }

    private async Task SetPermissionsAsync()
    {
        // FUTURE: Refactor this permission check.
        CanAddNewReview = ComplianceReview is null && await authorization.Succeeded(User, Policies.ComplianceStaff) &&
                          TestSummary is { ReportClosed: true };
        if (ComplianceReview is not null)
            UserCan = await authorization.SetPermissions(ComplianceOperation.AllOperations, User, ComplianceReview);
    }

    private async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetUsersInRoleAsync(AppRole.ComplianceStaffRole)).ToSelectList();
}
