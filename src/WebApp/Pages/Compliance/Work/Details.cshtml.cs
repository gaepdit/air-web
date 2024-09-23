using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using AirWeb.AppServices.Permissions;
using AirWeb.WebApp.Models;

namespace AirWeb.WebApp.Pages.Compliance.Work;

[Authorize(Policy = nameof(Policies.Staff))]
public class DetailsModel(IWorkEntryService entryService, IAuthorizationService authorization) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    public IWorkEntryViewDto Item { get; private set; } = null!;
    public CommentsSectionModel CommentSection { get; set; } = null!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    [TempData]
    public Guid NewCommentId { get; set; }

    [TempData]
    public string? NotificationFailureMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");

        var item = await entryService.FindAsync(Id);
        if (item is null) return NotFound();

        await SetPermissionsAsync(item);
        if (item.IsDeleted && !UserCan[ComplianceWorkOperation.ViewDeleted]) return NotFound();

        Item = item;
        CommentSection = new CommentsSectionModel
        {
            Comments = item.Comments,
            NewComment = new CommentAddDto(Id),
            NewCommentId = NewCommentId,
            NotificationFailureMessage = NotificationFailureMessage,
            CanAddComment = UserCan[ComplianceWorkOperation.AddComment],
            CanDeleteComment = UserCan[ComplianceWorkOperation.DeleteComment],
        };
        return Page();
    }

    public async Task<IActionResult> OnPostNewCommentAsync(CommentAddDto newComment,
        CancellationToken token)
    {
        var item = await entryService.FindAsync(Id, token);
        if (item is null || item.IsDeleted) return BadRequest();

        await SetPermissionsAsync(item);
        if (!UserCan[ComplianceWorkOperation.AddComment]) return BadRequest();

        if (!ModelState.IsValid)
        {
            Item = item;
            CommentSection = new CommentsSectionModel
            {
                Comments = item.Comments,
                NewComment = newComment,
                CanAddComment = UserCan[ComplianceWorkOperation.AddComment],
                CanDeleteComment = UserCan[ComplianceWorkOperation.DeleteComment],
            };
            return Page();
        }

        var addCommentResult = await entryService.AddCommentAsync(Id, newComment, token);
        NewCommentId = addCommentResult.CommentId;
        if (!addCommentResult.AppNotificationResult.Success)
            NotificationFailureMessage = addCommentResult.AppNotificationResult.FailureMessage;
        return RedirectToPage("Details", pageHandler: null, routeValues: new { Id }, fragment: NewCommentId.ToString());
    }

    public async Task<IActionResult> OnPostDeleteCommentAsync(Guid commentId, CancellationToken token)
    {
        var item = await entryService.FindAsync(Id, token);
        if (item is null || item.IsDeleted) return BadRequest();

        await SetPermissionsAsync(item);
        if (!UserCan[ComplianceWorkOperation.DeleteComment]) return BadRequest();

        await entryService.DeleteCommentAsync(commentId, token);
        return RedirectToPage("Details", pageHandler: null, routeValues: new { Id }, fragment: "comments");
    }


    private async Task SetPermissionsAsync(IWorkEntryViewDto item)
    {
        foreach (var operation in ComplianceWorkOperation.AllOperations)
            UserCan[operation] = (await authorization.AuthorizeAsync(User, item, operation)).Succeeded;
    }
}
