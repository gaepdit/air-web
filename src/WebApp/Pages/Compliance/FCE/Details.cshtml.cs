using AirWeb.AppServices.Compliance.Compliance.Fces;
using AirWeb.AppServices.Compliance.Compliance.Fces.SupportingData;
using AirWeb.AppServices.Compliance.Compliance.Permissions;
using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.AppServices.Core.EntityServices.Comments;
using AirWeb.WebApp.Models;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Compliance.FCE;

[Authorize(Policy = nameof(Policies.Staff))]
public class DetailsModel(IFceService fceService, IAuthorizationService authorization) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    public FceViewDto? FceView { get; private set; }
    public SupportingDataDetails? SupportingData { get; set; }
    public CommentsSectionModel CommentSection { get; set; } = null!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    [TempData]
    public Guid NewCommentId { get; set; }

    [TempData]
    public string? NotificationFailureMessage { get; set; }

    [TempData]
    public bool RefreshIaipData { get; set; }

    public async Task<IActionResult> OnGetAsync([FromQuery] bool refresh = false, CancellationToken token = default)
    {
        if (Id == 0) return RedirectToPage("Index");

        if (refresh)
        {
            RefreshIaipData = true;
            return RedirectToPage();
        }

        FceView = await fceService.FindAsync(Id, token);
        if (FceView is null) return NotFound();

        await SetPermissionsAsync();

        // FUTURE: Replace with ComplianceOperation.View? See Case File permissions for example. 
        //   And see `CompliancePermissions.CanView` for use.
        if (FceView.IsDeleted && !UserCan[ComplianceOperation.ViewDeleted]) return NotFound();

        SupportingData = await fceService.GetSupportingDetailsAsync((FacilityId)FceView.FacilityId,
            FceView.CompletedDate, RefreshIaipData, token);

        CommentSection = new CommentsSectionModel
        {
            Comments = FceView.Comments,
            NewComment = new CommentAddDto(Id),
            NewCommentId = NewCommentId,
            NotificationFailureMessage = NotificationFailureMessage,
            CanAddComment = UserCan[ComplianceOperation.AddComment],
            CanDeleteComment = UserCan[ComplianceOperation.DeleteComment],
        };
        return Page();
    }

    public async Task<IActionResult> OnPostNewCommentAsync(CommentAddDto newComment, CancellationToken token)
    {
        FceView = await fceService.FindAsync(Id, token);
        if (FceView is null || FceView.IsDeleted) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[ComplianceOperation.AddComment]) return BadRequest();

        if (!ModelState.IsValid)
        {
            SupportingData = await fceService.GetSupportingDetailsAsync((FacilityId)FceView.FacilityId,
                FceView.CompletedDate, token: token);

            CommentSection = new CommentsSectionModel
            {
                Comments = FceView.Comments,
                NewComment = newComment,
                CanAddComment = UserCan[ComplianceOperation.AddComment],
                CanDeleteComment = UserCan[ComplianceOperation.DeleteComment],
            };
            return Page();
        }

        var result = await fceService.AddCommentAsync(Id, newComment, token);
        NewCommentId = result.Id;
        if (result.HasWarning) TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, result.WarningMessage);
        return RedirectToPage("Details", pageHandler: null, fragment: NewCommentId.ToString());
    }

    public async Task<IActionResult> OnPostDeleteCommentAsync(Guid commentId, CancellationToken token)
    {
        FceView = await fceService.FindAsync(Id, token);
        if (FceView is null || FceView.IsDeleted) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[ComplianceOperation.DeleteComment]) return BadRequest();

        await fceService.DeleteCommentAsync(commentId, token);
        return RedirectToPage("Details", pageHandler: null, fragment: "comments");
    }

    private async Task SetPermissionsAsync() =>
        UserCan = await authorization.SetPermissions(ComplianceOperation.AllOperations, User, FceView);
}
