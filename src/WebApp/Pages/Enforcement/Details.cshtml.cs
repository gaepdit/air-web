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
    IValidator<MaxCurrentDateOnlyDto> issueActionValidator,
    IValidator<CommentAndMaxDateDto> addResponseValidator) : PageModel
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

    // Enforcement modal forms
    // Note: DTO names are referenced in the page JavaScript.

    [BindProperty]
    public CreateEnforcementActionDto CreateEnforcementAction { get; set; } = null!;

    [BindProperty]
    public MaxCurrentDateOnlyDto IssueEnforcementActionDate { get; set; } = null!;

    [BindProperty]
    public CommentAndMaxDateDto AddEnforcementActionResponse { get; set; } = null!;

    // Methods
    [TempData]
    public Guid? HighlightEnforcementId { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");
        Item = await caseFileService.FindDetailedAsync(Id);
        if (Item is null) return NotFound();

        await SetPermissionsAsync();
        if (!UserCan[EnforcementOperation.View]) return NotFound();

        return InitializePage(Item);
    }

    public async Task<IActionResult> OnPostAddEnforcementActionAsync(CancellationToken token)
    {
        var caseFile = await caseFileService.FindDetailedAsync(Id, token);
        if (caseFile is null || !User.CanEdit(caseFile)) return BadRequest();

        HighlightEnforcementId = await enforcementActionService.CreateAsync(Id, CreateEnforcementAction, token);
        return RedirectToFragment(HighlightEnforcementId.ToString()!);
    }

    public async Task<IActionResult> OnPostAddEnforcementActionResponseAsync(Guid enforcementActionId,
        CancellationToken token)
    {
        Item = await caseFileService.FindDetailedAsync(Id, token);
        if (Item is null || !User.CanEdit(Item)) return BadRequest();

        await addResponseValidator.ApplyValidationAsync(AddEnforcementActionResponse, ModelState);

        if (!ModelState.IsValid)
        {
            await SetPermissionsAsync();
            return InitializePage(Item);
        }

        await enforcementActionService.AddResponse(enforcementActionId, AddEnforcementActionResponse, token);
        HighlightEnforcementId = enforcementActionId;
        return RedirectToFragment(enforcementActionId.ToString());
    }

    public async Task<IActionResult> OnPostIssueEnforcementActionAsync(Guid enforcementActionId,
        CancellationToken token)
    {
        Item = await caseFileService.FindDetailedAsync(Id, token);
        if (Item is null || !User.CanEdit(Item)) return BadRequest();
        var action = Item.EnforcementActions.SingleOrDefault(actionViewDto => actionViewDto.Id == enforcementActionId);
        if (action is null || !User.CanFinalizeAction(action)) return BadRequest();

        await issueActionValidator.ApplyValidationAsync(IssueEnforcementActionDate, ModelState);

        if (!ModelState.IsValid)
        {
            await SetPermissionsAsync();
            return InitializePage(Item);
        }

        await enforcementActionService.IssueAsync(enforcementActionId, IssueEnforcementActionDate, token);
        HighlightEnforcementId = enforcementActionId;
        return RedirectToFragment(enforcementActionId.ToString());
    }

    public async Task<IActionResult> OnPostCancelEnforcementActionAsync(Guid enforcementActionId,
        CancellationToken token)
    {
        Item = await caseFileService.FindDetailedAsync(Id, token);
        if (Item is null || !User.CanEdit(Item)) return BadRequest();
        var action = Item.EnforcementActions.SingleOrDefault(actionViewDto => actionViewDto.Id == enforcementActionId);
        if (action is null || !User.CanFinalizeAction(action)) return BadRequest();

        await enforcementActionService.CancelAsync(enforcementActionId, token);
        HighlightEnforcementId = enforcementActionId;
        return RedirectToFragment(enforcementActionId.ToString());
    }

    public async Task<IActionResult> OnPostDeleteEnforcementActionAsync(Guid enforcementActionId,
        CancellationToken token)
    {
        Item = await caseFileService.FindDetailedAsync(Id, token);
        if (Item is null || !User.CanDelete(Item)) return BadRequest();
        var action = Item.EnforcementActions.SingleOrDefault(actionViewDto => actionViewDto.Id == enforcementActionId);
        if (action is null || User.CanDelete(action)) return BadRequest();

        await enforcementActionService.DeleteAsync(enforcementActionId, token);
        return RedirectToPage();
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
            return InitializePage(Item);
        }

        var addCommentResult = await caseFileService.AddCommentAsync(Id, newComment, token);
        NewCommentId = addCommentResult.Id;
        if (addCommentResult.AppNotificationResult is { Success: false })
            NotificationFailureMessage = addCommentResult.AppNotificationResult.FailureMessage;
        return RedirectToFragment(NewCommentId.ToString());
    }

    public async Task<IActionResult> OnPostDeleteCommentAsync(Guid commentId, CancellationToken token)
    {
        Item = await caseFileService.FindDetailedAsync(Id, token);
        if (Item is null || Item.IsDeleted) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[EnforcementOperation.DeleteComment]) return BadRequest();

        await caseFileService.DeleteCommentAsync(commentId, token);
        return RedirectToFragment("comments");
    }

    #endregion

    private async Task SetPermissionsAsync() =>
        UserCan = await authorization.SetPermissions(EnforcementOperation.AllOperations, User, Item);

    private PageResult InitializePage(CaseFileViewDto item, CommentAddDto? newComment = null)
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
        AddEnforcementActionResponse = new CommentAndMaxDateDto();

        return Page();
    }

    private RedirectToPageResult RedirectToFragment(string fragment) =>
        RedirectToPage("Details", pageHandler: null, fragment: fragment);
}
