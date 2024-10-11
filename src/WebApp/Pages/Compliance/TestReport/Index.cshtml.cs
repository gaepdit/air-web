using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.WebApp.Models;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace AirWeb.WebApp.Pages.Compliance.TestReport;

[Authorize(Policy = nameof(Policies.Staff))]
public class IndexModel(
    ISourceTestService testService,
    IWorkEntryService entryService,
    IAuthorizationService authorization) : PageModel
{
    [FromRoute]
    public int ReferenceNumber { get; set; }

    public SourceTestSummary? Item { get; private set; }
    public SourceTestReviewViewDto? ComplianceReview { get; private set; }
    public CommentsSectionModel? CommentSection { get; set; }
    public bool IsComplianceStaff { get; private set; }
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    [TempData]
    public Guid NewCommentId { get; set; }

    [TempData]
    public string? NotificationFailureMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        if (ReferenceNumber > 0) Item = await testService.FindSummaryAsync(ReferenceNumber);
        if (Item is null) return NotFound();

        ComplianceReview = await entryService.FindSourceTestReviewAsync(ReferenceNumber, token);
        await SetPermissionsAsync();

        if (ComplianceReview is not null)
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

    private async Task SetPermissionsAsync()
    {
        IsComplianceStaff = await authorization.Succeeded(User, Policies.ComplianceStaff);
        UserCan = await authorization.SetPermissions(ComplianceWorkOperation.AllOperations, User, ComplianceReview);
    }
}
