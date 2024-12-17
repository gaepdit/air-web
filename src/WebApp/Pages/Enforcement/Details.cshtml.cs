using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.AppServices.Enforcement.Query;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.WebApp.Models;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(Policies.Staff))]
public class DetailsModel(IEnforcementService enforcementService, IAuthorizationService authorization) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    public CaseFileViewDto? Item { get; private set; }
    public CommentsSectionModel CommentSection { get; set; } = null!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    [TempData]
    public Guid NewCommentId { get; set; }

    [TempData]
    public string? NotificationFailureMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");
        Item = await enforcementService.FindAsync(Id);
        if (Item is null) return NotFound();

        await SetPermissionsAsync();
        if (Item.IsDeleted && !UserCan[EnforcementOperation.ViewDeleted]) return NotFound();
        if (!Item.IsClosed && !UserCan[EnforcementOperation.View]) return NotFound();

        CommentSection = new CommentsSectionModel
        {
            Comments = Item.Comments,
            NewComment = new CommentAddDto(Id),
            NewCommentId = NewCommentId,
            NotificationFailureMessage = NotificationFailureMessage,
            CanAddComment = UserCan[EnforcementOperation.AddComment],
            CanDeleteComment = UserCan[EnforcementOperation.DeleteComment],
        };
        return Page();
    }

    public async Task<IActionResult> OnPostNewCommentAsync(CommentAddDto newComment,
        CancellationToken token)
    {
        Item = await enforcementService.FindAsync(Id, token);
        if (Item is null || Item.IsDeleted) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[EnforcementOperation.AddComment]) return BadRequest();

        if (!ModelState.IsValid)
        {
            CommentSection = new CommentsSectionModel
            {
                Comments = Item.Comments,
                NewComment = newComment,
                CanAddComment = UserCan[EnforcementOperation.AddComment],
                CanDeleteComment = UserCan[EnforcementOperation.DeleteComment],
            };
            return Page();
        }

        var addCommentResult = await enforcementService.AddCommentAsync(Id, newComment, token);
        NewCommentId = addCommentResult.Id;
        if (addCommentResult.AppNotificationResult is { Success: false })
            NotificationFailureMessage = addCommentResult.AppNotificationResult.FailureMessage;
        return RedirectToPage("Details", pageHandler: null, routeValues: new { Id }, fragment: NewCommentId.ToString());
    }

    public async Task<IActionResult> OnPostDeleteCommentAsync(Guid commentId, CancellationToken token)
    {
        Item = await enforcementService.FindAsync(Id, token);
        if (Item is null || Item.IsDeleted) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[EnforcementOperation.DeleteComment]) return BadRequest();

        await enforcementService.DeleteCommentAsync(commentId, token);
        return RedirectToPage("Details", pageHandler: null, routeValues: new { Id }, fragment: "comments");
    }

    private async Task SetPermissionsAsync() =>
        UserCan = await authorization.SetPermissions(EnforcementOperation.AllOperations, User, Item);
}
