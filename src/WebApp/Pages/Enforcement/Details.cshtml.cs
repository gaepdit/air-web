using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.AppServices.Core.CommonDtos;
using AirWeb.AppServices.Core.EntityServices.Comments;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.WebApp.Models;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(Policies.Staff))]
public class DetailsModel(
    ICaseFileService caseFileService,
    IEnforcementActionService actionService,
    IAuthorizationService authorization,
    IValidator<MaxDateOnlyDto> maxDateValidator,
    IValidator<MaxDateAndCommentDto> addResponseValidator,
    IValidator<MaxDateAndBooleanDto> resolveActionValidator) : PageModel
{
    // Case File
    [FromRoute]
    public int Id { get; set; } // Case File ID

    public CaseFileViewDto? CaseFile { get; private set; }
    public CommentsSectionModel CommentSection { get; set; } = null!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    [TempData]
    public string? NotificationFailureMessage { get; set; }

    [TempData]
    public Guid NewCommentId { get; set; }

    // Enforcement modal forms
    // Note: DTO names are referenced in the page JavaScript.

    [BindProperty]
    public EnforcementActionCreateDto CreateEnforcementAction { get; set; } = null!;

    [BindProperty]
    public MaxDateAndBooleanDto IssueEnforcementAction { get; set; } = null!;

    [BindProperty]
    public MaxDateAndCommentDto AddEnforcementActionResponse { get; set; } = null!;

    [BindProperty]
    public MaxDateOnlyDto ExecuteOrder { get; set; } = null!;

    [BindProperty]
    public MaxDateOnlyDto AppealOrder { get; set; } = null!;

    [BindProperty]
    public MaxDateAndBooleanDto ResolveEnforcementAction { get; set; } = null!;

    [TempData]
    public Guid? HighlightEnforcementId { get; set; }

    // Methods
    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");
        CaseFile = await caseFileService.FindDetailedAsync(Id);
        if (CaseFile is null) return NotFound();

        await SetPermissionsAsync();
        if (CaseFile.IsDeleted && !UserCan[CaseFileOperation.ViewDeleted]) return NotFound();
        if (!UserCan[CaseFileOperation.View]) return Forbid();

        if (!UserCan[CaseFileOperation.ViewDraftEnforcement])
            CaseFile.EnforcementActions.RemoveAll(dto => !dto.IsIssued);

        return InitializePage();
    }

    public async Task<IActionResult> OnPostAddEnforcementActionAsync(CancellationToken token)
    {
        var caseFile = await caseFileService.FindDetailedAsync(Id, token);
        if (caseFile is null || !User.CanEditCaseFile(caseFile)) return BadRequest();

        HighlightEnforcementId = await actionService.CreateAsync(Id, CreateEnforcementAction, token);

        return (caseFile.MissingPollutantsOrPrograms || caseFile.MissingViolationType) &&
               CreateEnforcementAction.IsReportableAction
            ? RedirectToPage("PollutantsPrograms", new { Id })
            : RedirectToFragment(HighlightEnforcementId.ToString()!);
    }

    public async Task<IActionResult> OnPostAddEnforcementActionResponseAsync(Guid enforcementActionId,
        CancellationToken token)
    {
        CaseFile = await caseFileService.FindDetailedAsync(Id, token);
        if (CaseFile is null || !User.CanEditCaseFile(CaseFile)) return BadRequest();

        await addResponseValidator.ApplyValidationAsync(AddEnforcementActionResponse, ModelState);

        if (!ModelState.IsValid)
        {
            await SetPermissionsAsync();
            return InitializePage();
        }

        await actionService.AddResponse(enforcementActionId, AddEnforcementActionResponse, token);
        HighlightEnforcementId = enforcementActionId;
        return RedirectToFragment(enforcementActionId.ToString());
    }

    public async Task<IActionResult> OnPostIssueEnforcementActionAsync(Guid enforcementActionId,
        CancellationToken token)
    {
        CaseFile = await caseFileService.FindDetailedAsync(Id, token);
        if (CaseFile is null || !User.CanEditCaseFile(CaseFile)) return BadRequest();
        var action = CaseFile.EnforcementActions.SingleOrDefault(dto => dto.Id == enforcementActionId);
        if (action is null || !User.CanFinalizeAction(action)) return BadRequest();

        await SetPermissionsAsync();

        await maxDateValidator.ApplyValidationAsync(IssueEnforcementAction, ModelState);
        if (!ModelState.IsValid)
        {
            return InitializePage();
        }

        var closeCaseFileWasSet = IssueEnforcementAction.Option;
        IssueEnforcementAction.Option = IssueEnforcementAction.Option && UserCan[CaseFileOperation.CloseCaseFile];

        var caseFileClosed = await actionService.IssueAsync(enforcementActionId, IssueEnforcementAction, token);
        if (caseFileClosed) return RedirectToFragment(null);

        if (closeCaseFileWasSet)
        {
            TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning,
                "The Enforcement Case could not be closed.");
        }

        HighlightEnforcementId = enforcementActionId;

        return CaseFile.LacksPollutantsOrPrograms
            ? RedirectToPage("PollutantsPrograms", new { Id })
            : RedirectToFragment(enforcementActionId.ToString());
    }

    public async Task<IActionResult> OnPostCancelEnforcementActionAsync(Guid enforcementActionId,
        CancellationToken token)
    {
        var action = await actionService.FindAsync(enforcementActionId, token);
        if (action is null || !User.CanFinalizeAction(action)) return BadRequest();

        await actionService.CancelAsync(enforcementActionId, token);
        HighlightEnforcementId = enforcementActionId;
        return RedirectToFragment(enforcementActionId.ToString());
    }

    public async Task<IActionResult> OnPostExecuteOrderAsync(Guid enforcementActionId, CancellationToken token)
    {
        var action = await actionService.FindAsync(enforcementActionId, token);
        if (action is null || !action.CanBeExecuted() || !User.CanEdit(action)) return BadRequest();

        await maxDateValidator.ApplyValidationAsync(ExecuteOrder, ModelState);

        if (!ModelState.IsValid)
        {
            await SetPermissionsAsync();
            return InitializePage();
        }

        await actionService.ExecuteOrderAsync(enforcementActionId, ExecuteOrder, token);
        HighlightEnforcementId = enforcementActionId;
        return RedirectToFragment(enforcementActionId.ToString());
    }

    public async Task<IActionResult> OnPostAppealOrderAsync(Guid enforcementActionId, CancellationToken token)
    {
        var action = await actionService.FindAsync(enforcementActionId, token);
        if (action is null || !action.CanBeAppealed() || !User.CanEdit(action)) return BadRequest();

        await maxDateValidator.ApplyValidationAsync(AppealOrder, ModelState);

        if (!ModelState.IsValid)
        {
            await SetPermissionsAsync();
            return InitializePage();
        }

        await actionService.AppealOrderAsync(enforcementActionId, AppealOrder, token);
        HighlightEnforcementId = enforcementActionId;
        return RedirectToFragment(enforcementActionId.ToString());
    }

    public async Task<IActionResult> OnPostResolveActionAsync(Guid enforcementActionId, CancellationToken token)
    {
        CaseFile = await caseFileService.FindDetailedAsync(Id, token);
        if (CaseFile is null || !User.CanEditCaseFile(CaseFile)) return BadRequest();
        var action = CaseFile.EnforcementActions.SingleOrDefault(dto => dto.Id == enforcementActionId);
        if (action is null || !User.CanResolve(action)) return BadRequest();

        await SetPermissionsAsync();

        await resolveActionValidator.ApplyValidationAsync(ResolveEnforcementAction, ModelState);
        if (!ModelState.IsValid)
        {
            return InitializePage();
        }

        var closeCaseFileWasSet = ResolveEnforcementAction.Option;
        ResolveEnforcementAction.Option = ResolveEnforcementAction.Option && UserCan[CaseFileOperation.CloseCaseFile];
        var caseFileClosed = await actionService.ResolveAsync(enforcementActionId, ResolveEnforcementAction, token);
        if (caseFileClosed) return RedirectToFragment(null);

        if (closeCaseFileWasSet)
        {
            TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning,
                "The Enforcement Case could not be closed.");
        }

        HighlightEnforcementId = enforcementActionId;
        return RedirectToFragment(enforcementActionId.ToString());
    }

    public async Task<IActionResult> OnPostDeleteEnforcementActionAsync(Guid enforcementActionId,
        CancellationToken token)
    {
        var action = await actionService.FindAsync(enforcementActionId, token);
        if (action is null || !User.CanDeleteAction(action)) return BadRequest();

        await actionService.DeleteAsync(enforcementActionId, action.CaseFileId, token);
        return RedirectToPage();
    }

    #region Comments

    public async Task<IActionResult> OnPostNewCommentAsync(CommentAddDto newComment, CancellationToken token)
    {
        CaseFile = await caseFileService.FindDetailedAsync(Id, token);
        if (CaseFile is null || CaseFile.IsDeleted) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[CaseFileOperation.AddComment]) return BadRequest();

        if (!ModelState.IsValid)
        {
            return InitializePage();
        }

        var result = await caseFileService.AddCommentAsync(Id, newComment, token);
        NewCommentId = result.Id;
        if (result.HasWarning) TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, result.WarningMessage);
        return RedirectToFragment(NewCommentId.ToString());
    }

    public async Task<IActionResult> OnPostDeleteCommentAsync(Guid commentId, CancellationToken token)
    {
        var caseFileSummary = await caseFileService.FindSummaryAsync(Id, token);
        if (!(await authorization.AuthorizeAsync(User, caseFileSummary, requirement: CaseFileOperation.DeleteComment))
            .Succeeded)
            return BadRequest();

        await caseFileService.DeleteCommentAsync(commentId, token);
        return RedirectToFragment("comments");
    }

    #endregion

    private async Task SetPermissionsAsync() =>
        UserCan = await authorization.SetPermissions(CaseFileOperation.AllOperations, User, CaseFile);

    private PageResult InitializePage(CommentAddDto? newComment = null)
    {
        CommentSection = new CommentsSectionModel
        {
            Comments = CaseFile!.Comments,
            NewComment = newComment ?? new CommentAddDto(Id),
            NewCommentId = NewCommentId,
            NotificationFailureMessage = NotificationFailureMessage,
            CanAddComment = UserCan[CaseFileOperation.AddComment],
            CanDeleteComment = UserCan[CaseFileOperation.DeleteComment],
        };

        CreateEnforcementAction = new EnforcementActionCreateDto();
        IssueEnforcementAction = new MaxDateAndBooleanDto
            { Option = UserCan[CaseFileOperation.CloseCaseFile] && !CaseFile.MissingData };
        AddEnforcementActionResponse = new MaxDateAndCommentDto();
        ExecuteOrder = new MaxDateOnlyDto();
        AppealOrder = new MaxDateOnlyDto();
        ResolveEnforcementAction = new MaxDateAndBooleanDto
            { Option = UserCan[CaseFileOperation.CloseCaseFile] && !CaseFile.AttentionNeeded };

        return Page();
    }

    private RedirectToPageResult RedirectToFragment(string? fragment) =>
        RedirectToPage("Details", pageHandler: null, fragment: fragment);
}
