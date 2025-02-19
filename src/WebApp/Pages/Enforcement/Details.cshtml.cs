using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(Policies.Staff))]
public class DetailsModel(
    ICaseFileService caseFileService,
    IEnforcementActionService enforcementActionService,
    IAuthorizationService authorization,
    IValidator<MaxCurrentDateOnlyDto> validator) : PageModel
{
    // Case File
    [FromRoute]
    public int Id { get; set; }

    public CaseFileViewDto? Item { get; private set; }

    // Permissions, etc.
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    [TempData]
    public string? NotificationFailureMessage { get; set; }

    // Comments
    public CommentsSectionModel CommentSection { get; set; } = null!;

    [TempData]
    public Guid NewCommentId { get; set; }

    // Enforcement
    [BindProperty]
    // Note: This name is reference in the page JavaScript.
    public CreateEnforcementActionDto CreateEnforcementAction { get; set; } = null!;

    [BindProperty]
    // Note: This name is reference in the page JavaScript.
    public MaxCurrentDateOnlyDto IssueEnforcementActionDate { get; set; } = null!;

    [TempData]
    public Guid? NewEnforcementId { get; set; }

    // Methods
    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");
        Item = await caseFileService.FindDetailedAsync(Id);
        if (Item is null) return NotFound();

        await SetPermissionsAsync();
        if (Item.IsDeleted && !UserCan[EnforcementOperation.ViewDeleted]) return NotFound();
        if (!Item.IsClosed && !UserCan[EnforcementOperation.View]) return NotFound();

        InitializeDtos(Item);
        return Page();
    }

    public async Task<IActionResult> OnPostAddEnforcementActionAsync(CancellationToken token)
    {
        var caseFile = await caseFileService.FindDetailedAsync(Id, token);
        if (caseFile is null || !User.CanEdit(caseFile)) return BadRequest();

        NewEnforcementId = await enforcementActionService.CreateAsync(Id, CreateEnforcementAction, token);
        return RedirectToPage("Details", pageHandler: null, fragment: NewEnforcementId.ToString());
    }

    public async Task<IActionResult> OnPostIssueEnforcementActionAsync(Guid enforcementActionId,
        CancellationToken token)
    {
        Item = await caseFileService.FindDetailedAsync(Id, token);
        if (Item is null || !User.CanEdit(Item)) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[EnforcementOperation.Edit]) return BadRequest();

        await validator.ApplyValidationAsync(IssueEnforcementActionDate, ModelState);

        if (!ModelState.IsValid)
        {
            InitializeDtos(Item);
            return Page();
        }

        await enforcementActionService.IssueAsync(enforcementActionId, IssueEnforcementActionDate, token);
        return RedirectToPage("Details", pageHandler: null, fragment: enforcementActionId.ToString());
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
            InitializeDtos(Item, newComment);
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

    private void InitializeDtos(CaseFileViewDto item, CommentAddDto? newComment = null)
    {
        CommentSection = new CommentsSectionModel
        {
            Comments = item.Comments,
            NewComment = newComment ?? new CommentAddDto(Id),
            NewCommentId = NewCommentId,
            NotificationFailureMessage = NotificationFailureMessage,
            CanAddComment = UserCan[EnforcementOperation.AddComment],
            CanDeleteComment = UserCan[EnforcementOperation.DeleteComment],
        };

        CreateEnforcementAction = new CreateEnforcementActionDto();
        IssueEnforcementActionDate = new MaxCurrentDateOnlyDto();
    }
}
