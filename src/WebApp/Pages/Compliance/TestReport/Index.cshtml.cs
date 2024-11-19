using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.Staff;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace AirWeb.WebApp.Pages.Compliance.TestReport;

[Authorize(Policy = nameof(Policies.Staff))]
public class IndexModel(
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

    [TempData]
    public Guid NewCommentId { get; set; }

    [TempData]
    public string? NotificationFailureMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        if (ReferenceNumber > 0)
            TestSummary = await testService.FindSummaryAsync(ReferenceNumber);
        if (TestSummary is null) return NotFound();

        ComplianceReview = await entryService.FindSourceTestReviewAsync(ReferenceNumber, token);
        await SetPermissionsAsync();

        if (ComplianceReview is null)
        {
            NewComplianceReview = new SourceTestReviewCreateDto
            {
                ReferenceNumber = ReferenceNumber,
                TestReportIsClosed = TestSummary.ReportClosed,
                FacilityId = TestSummary.Facility?.FacilityId,
                ResponsibleStaffId = (await staffService.GetCurrentUserAsync()).Id,
            };
            await PopulateSelectListsAsync();
        }
        else
        {
            CommentSection = new CommentsSectionModel
            {
                Comments = ComplianceReview.Comments,
                NewComment = new CommentAddDto(ComplianceReview.Id),
                NewCommentId = NewCommentId,
                NotificationFailureMessage = NotificationFailureMessage,
                CanAddComment = UserCan[ComplianceWorkOperation.AddComment],
                CanDeleteComment = UserCan[ComplianceWorkOperation.DeleteComment],
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

        var createResult = await entryService.CreateAsync(newComplianceReview, token);

        const string message = "Compliance Review successfully created.";
        if (createResult.HasAppNotificationFailure)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning, message,
                createResult.AppNotificationResult!.FailureMessage);
        }
        else
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, message);
        }

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
        if (!UserCan[ComplianceWorkOperation.AddComment]) return BadRequest();

        if (!ModelState.IsValid)
        {
            CommentSection = new CommentsSectionModel
            {
                Comments = ComplianceReview.Comments,
                NewComment = newComment,
                CanAddComment = UserCan[ComplianceWorkOperation.AddComment],
                CanDeleteComment = UserCan[ComplianceWorkOperation.DeleteComment],
            };
            return Page();
        }

        var addCommentResult = await entryService.AddCommentAsync(ComplianceReview.Id, newComment, token);
        NewCommentId = addCommentResult.Id;
        if (addCommentResult.AppNotificationResult is { Success: false })
            NotificationFailureMessage = addCommentResult.AppNotificationResult.FailureMessage;

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
        if (!UserCan[ComplianceWorkOperation.DeleteComment]) return BadRequest();

        await entryService.DeleteCommentAsync(commentId, token);
        return RedirectToPage("Index", pageHandler: null, routeValues: new { ReferenceNumber },
            fragment: "comments");
    }

    private async Task SetPermissionsAsync()
    {
        CanAddNewReview = await authorization.Succeeded(User, Policies.ComplianceStaff) &&
                          TestSummary is { ReportClosed: true } && ComplianceReview is null;
        UserCan = await authorization.SetPermissions(ComplianceWorkOperation.AllOperations, User, ComplianceReview);
    }

    private async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();
}
