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
    public SourceTestReviewViewDto? SourceTestReview { get; private set; }
    public CommentsSectionModel? CommentSection { get; set; }
    public SourceTestReviewCreateDto? NewReview { get; set; }
    public SelectList? StaffSelectList { get; private set; }

    // Permissions
    public bool IsComplianceStaff { get; private set; }

    // This "UserCan" dictionary is only used if a compliance review (`SourceTestReview`) exists.
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    [TempData]
    public Guid NewCommentId { get; set; }

    [TempData]
    public string? NotificationFailureMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        if (ReferenceNumber > 0) TestSummary = await testService.FindSummaryAsync(ReferenceNumber);
        if (TestSummary is null) return NotFound();

        SourceTestReview = await entryService.FindSourceTestReviewAsync(ReferenceNumber, token);
        await SetPermissionsAsync();

        if (SourceTestReview is null)
        {
            NewReview = new SourceTestReviewCreateDto
            {
                ReferenceNumber = ReferenceNumber,
                FacilityId = TestSummary.Facility?.FacilityId,
                ResponsibleStaffId = (await staffService.GetCurrentUserAsync()).Id,
            };
            await PopulateSelectListsAsync();
        }
        else
        {
            CommentSection = new CommentsSectionModel
            {
                Comments = SourceTestReview.Comments,
                NewComment = new CommentAddDto(SourceTestReview.Id),
                NewCommentId = NewCommentId,
                NotificationFailureMessage = NotificationFailureMessage,
                CanAddComment = UserCan[ComplianceWorkOperation.AddComment],
                CanDeleteComment = UserCan[ComplianceWorkOperation.DeleteComment],
            };
        }

        return Page();
    }

    public async Task<IActionResult> OnPostNewReviewAsync(SourceTestReviewCreateDto newReview, CancellationToken token)
    {
        if (newReview.FacilityId == null || newReview.ReferenceNumber == 0 ||
            ReferenceNumber != newReview.ReferenceNumber)
            return BadRequest();

        await SetPermissionsAsync();
        if (!IsComplianceStaff) return BadRequest();

        await validator.ApplyValidationAsync(newReview, ModelState);

        if (!ModelState.IsValid)
        {
            TestSummary = await testService.FindSummaryAsync(ReferenceNumber);
            if (TestSummary is null) return BadRequest();

            await PopulateSelectListsAsync();
            return Page();
        }

        var createResult = await entryService.CreateAsync(newReview, token);

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
        SourceTestReview = await entryService.FindSourceTestReviewAsync(ReferenceNumber, token);
        if (SourceTestReview is null || SourceTestReview.IsDeleted) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[ComplianceWorkOperation.AddComment]) return BadRequest();

        if (!ModelState.IsValid)
        {
            TestSummary = await testService.FindSummaryAsync(ReferenceNumber);
            if (TestSummary is null) return BadRequest();

            CommentSection = new CommentsSectionModel
            {
                Comments = SourceTestReview.Comments,
                NewComment = newComment,
                CanAddComment = UserCan[ComplianceWorkOperation.AddComment],
                CanDeleteComment = UserCan[ComplianceWorkOperation.DeleteComment],
            };
            return Page();
        }

        var addCommentResult = await entryService.AddCommentAsync(SourceTestReview.Id, newComment, token);
        NewCommentId = addCommentResult.Id;
        if (addCommentResult.AppNotificationResult is { Success: false })
            NotificationFailureMessage = addCommentResult.AppNotificationResult.FailureMessage;

        return RedirectToPage("Index", pageHandler: null, routeValues: new { ReferenceNumber },
            fragment: NewCommentId.ToString());
    }

    public async Task<IActionResult> OnPostDeleteCommentAsync(Guid commentId, CancellationToken token)
    {
        SourceTestReview = await entryService.FindSourceTestReviewAsync(ReferenceNumber, token);
        if (SourceTestReview is null || SourceTestReview.IsDeleted) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[ComplianceWorkOperation.DeleteComment]) return BadRequest();

        await entryService.DeleteCommentAsync(commentId, token);
        return RedirectToPage("Index", pageHandler: null, routeValues: new { ReferenceNumber },
            fragment: "comments");
    }

    private async Task SetPermissionsAsync()
    {
        IsComplianceStaff = await authorization.Succeeded(User, Policies.ComplianceStaff);
        UserCan = await authorization.SetPermissions(ComplianceWorkOperation.AllOperations, User, SourceTestReview);
    }

    private async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();
}
