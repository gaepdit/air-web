using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.WebApp.Models;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(Policies.Staff))]
public class DetailsModel(
    ICaseFileService caseFileService,
    IEnforcementActionService enforcementActionService,
    IAuthorizationService authorization) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty]
    // FYI, this name is reference in the page JavaScript.
    public CreateEnforcementActionDto CreateEnforcementAction { get; set; } = null!;

    public CaseFileViewDto? Item { get; private set; }
    public CommentsSectionModel CommentSection { get; set; } = null!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    [TempData]
    public Guid? NewEnforcementId { get; set; }

    [TempData]
    public Guid NewCommentId { get; set; }

    [TempData]
    public string? NotificationFailureMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");
        Item = await caseFileService.FindDetailedAsync(Id);
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
        CreateEnforcementAction = new();
        return Page();
    }

    public async Task<IActionResult> OnPostAddEnforcementActionAsync(CancellationToken token)
    {
        var caseFile = await caseFileService.FindDetailedAsync(Id, token);
        if (caseFile is null || !User.CanEdit(caseFile)) return BadRequest();

        NewEnforcementId = await enforcementActionService.CreateAsync(Id, CreateEnforcementAction, token);
        return RedirectToPage("Details", pageHandler: null, fragment: NewEnforcementId.ToString());
    }

    #region Comments

    public async Task<IActionResult> OnPostNewCommentAsync(CommentAddDto newComment,
        CancellationToken token)
    {
        Item = await caseFileService.FindDetailedAsync(Id, token);
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
            CreateEnforcementAction = new();
            return Page();
        }

        var addCommentResult = await caseFileService.AddCommentAsync(Id, newComment, token);
        NewCommentId = addCommentResult.Id;
        if (addCommentResult.AppNotificationResult is { Success: false })
            NotificationFailureMessage = addCommentResult.AppNotificationResult.FailureMessage;
        return RedirectToPage("Details", pageHandler: null, fragment: NewCommentId.ToString());
    }

    public async Task<IActionResult> OnPostDeleteCommentAsync(Guid commentId, CancellationToken token)
    {
        Item = await caseFileService.FindDetailedAsync(Id, token);
        if (Item is null || Item.IsDeleted) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[EnforcementOperation.DeleteComment]) return BadRequest();

        await caseFileService.DeleteCommentAsync(commentId, token);
        return RedirectToPage("Details", pageHandler: null, fragment: "comments");
    }

    #endregion

    private async Task SetPermissionsAsync() =>
        UserCan = await authorization.SetPermissions(EnforcementOperation.AllOperations, User, Item);
}
