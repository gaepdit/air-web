using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.Staff;
using AirWeb.WebApp.Models;
using GaEpd.AppLibrary.ListItems;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace AirWeb.WebApp.Pages.Compliance.TestReport;

[Authorize(Policy = nameof(Policies.Staff))]
public class IndexModel(
    ISourceTestService testService,
    IWorkEntryService entryService,
    IStaffService staffService,
    IAuthorizationService authorization) : PageModel
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

    private async Task SetPermissionsAsync()
    {
        IsComplianceStaff = await authorization.Succeeded(User, Policies.ComplianceStaff);
        UserCan = await authorization.SetPermissions(ComplianceWorkOperation.AllOperations, User, SourceTestReview);
    }

    private async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();
}
