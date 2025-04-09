using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.ComplianceStaff;
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
    IValidator<MaxDateOnlyDto> maxDateValidator,
    IValidator<MaxDateAndCommentDto> addResponseValidator,
    IValidator<MaxDateAndBooleanDto> resolveActionValidator) : PageModel
{
    // Case File
    [FromRoute]
    public int Id { get; set; } // Case File ID

    public CaseFileViewDto? CaseFile { get; private set; }

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
        if (!UserCan[EnforcementOperation.View]) return NotFound();

        return InitializePage();
    }

    public async Task<IActionResult> OnPostAddEnforcementActionAsync(CancellationToken token)
    {
        var caseFile = await caseFileService.FindDetailedAsync(Id, token);
        if (caseFile is null || !User.CanEdit(caseFile)) return BadRequest();

        HighlightEnforcementId = await enforcementActionService.CreateAsync(Id, CreateEnforcementAction, token);

        return caseFile.MissingPollutantsOrPrograms && CreateEnforcementAction.WouldBeReportable
            ? RedirectToPage("PollutantsPrograms", new { Id })
            : RedirectToFragment(HighlightEnforcementId.ToString()!);
    }

    public async Task<IActionResult> OnPostAddEnforcementActionResponseAsync(Guid enforcementActionId,
        CancellationToken token)
    {
        CaseFile = await caseFileService.FindDetailedAsync(Id, token);
        if (CaseFile is null || !User.CanEdit(CaseFile)) return BadRequest();

        await addResponseValidator.ApplyValidationAsync(AddEnforcementActionResponse, ModelState);

        if (!ModelState.IsValid)
        {
            await SetPermissionsAsync();
            return InitializePage();
        }

        await enforcementActionService.AddResponse(enforcementActionId, AddEnforcementActionResponse, token);
        HighlightEnforcementId = enforcementActionId;
        return RedirectToFragment(enforcementActionId.ToString());
    }

    public async Task<IActionResult> OnPostIssueEnforcementActionAsync(Guid enforcementActionId,
        CancellationToken token)
    {
        CaseFile = await caseFileService.FindDetailedAsync(Id, token);
        if (CaseFile is null || !User.CanEdit(CaseFile)) return BadRequest();
        var action = CaseFile.EnforcementActions.SingleOrDefault(dto => dto.Id == enforcementActionId);
        if (action is null || !User.CanFinalizeAction(action)) return BadRequest();

        await maxDateValidator.ApplyValidationAsync(IssueEnforcementAction, ModelState);

        if (!ModelState.IsValid)
        {
            await SetPermissionsAsync();
            return InitializePage();
        }

        bool caseFileClosed =
            await enforcementActionService.IssueAsync(enforcementActionId, IssueEnforcementAction, token);

        if (caseFileClosed) return RedirectToFragment(null);

        if (IssueEnforcementAction.Option)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "The Enforcement Case could not be closed.");
        }

        HighlightEnforcementId = enforcementActionId;

        return CaseFile.WillRequirePollutantsOrPrograms
            ? RedirectToPage("PollutantsPrograms", new { Id })
            : RedirectToFragment(enforcementActionId.ToString());
    }

    public async Task<IActionResult> OnPostCancelEnforcementActionAsync(Guid enforcementActionId,
        CancellationToken token)
    {
        var action = await enforcementActionService.FindAsync(enforcementActionId, token);
        if (action is null || !User.CanFinalizeAction(action)) return BadRequest();

        await enforcementActionService.CancelAsync(enforcementActionId, token);
        HighlightEnforcementId = enforcementActionId;
        return RedirectToFragment(enforcementActionId.ToString());
    }

    public async Task<IActionResult> OnPostExecuteOrderAsync(Guid enforcementActionId, CancellationToken token)
    {
        var action = await enforcementActionService.FindAsync(enforcementActionId, token);
        if (action is null || !action.CanBeExecuted()) return BadRequest();

        await maxDateValidator.ApplyValidationAsync(ExecuteOrder, ModelState);

        if (!ModelState.IsValid)
        {
            await SetPermissionsAsync();
            return InitializePage();
        }

        await enforcementActionService.ExecuteOrderAsync(enforcementActionId, ExecuteOrder, token);
        HighlightEnforcementId = enforcementActionId;
        return RedirectToFragment(enforcementActionId.ToString());
    }

    public async Task<IActionResult> OnPostAppealOrderAsync(Guid enforcementActionId, CancellationToken token)
    {
        var action = await enforcementActionService.FindAsync(enforcementActionId, token);
        if (action is null || !action.CanBeAppealed()) return BadRequest();

        await maxDateValidator.ApplyValidationAsync(AppealOrder, ModelState);

        if (!ModelState.IsValid)
        {
            await SetPermissionsAsync();
            return InitializePage();
        }

        await enforcementActionService.AppealOrderAsync(enforcementActionId, AppealOrder, token);
        HighlightEnforcementId = enforcementActionId;
        return RedirectToFragment(enforcementActionId.ToString());
    }

    public async Task<IActionResult> OnPostResolveActionAsync(Guid enforcementActionId, CancellationToken token)
    {
        CaseFile = await caseFileService.FindDetailedAsync(Id, token);
        if (CaseFile is null || !User.CanEdit(CaseFile)) return BadRequest();
        var action = CaseFile.EnforcementActions.SingleOrDefault(dto => dto.Id == enforcementActionId);
        if (action is null || !User.CanResolve(action)) return BadRequest();

        await resolveActionValidator.ApplyValidationAsync(ResolveEnforcementAction, ModelState);

        if (!ModelState.IsValid)
        {
            await SetPermissionsAsync();
            return InitializePage();
        }

        bool caseFileClosed =
            await enforcementActionService.ResolveAsync(enforcementActionId, ResolveEnforcementAction, token);

        if (caseFileClosed) return RedirectToFragment(null);

        if (IssueEnforcementAction.Option)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "The Enforcement Case could not be closed.");
        }

        HighlightEnforcementId = enforcementActionId;
        return RedirectToFragment(enforcementActionId.ToString());
    }

    public async Task<IActionResult> OnPostDeleteEnforcementActionAsync(Guid enforcementActionId,
        CancellationToken token)
    {
        var action = await enforcementActionService.FindAsync(enforcementActionId, token);
        if (action is null || !User.CanDelete(action)) return BadRequest();

        await enforcementActionService.DeleteAsync(enforcementActionId, token);
        return RedirectToPage();
    }

    #region Comments

    public async Task<IActionResult> OnPostNewCommentAsync(CommentAddDto newComment, CancellationToken token)
    {
        CaseFile = await caseFileService.FindDetailedAsync(Id, token);
        if (CaseFile is null || CaseFile.IsDeleted) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[EnforcementOperation.AddComment]) return BadRequest();

        if (!ModelState.IsValid) return InitializePage();

        var addCommentResult = await caseFileService.AddCommentAsync(Id, newComment, token);
        NewCommentId = addCommentResult.Id;
        if (addCommentResult.AppNotificationResult is { Success: false })
            NotificationFailureMessage = addCommentResult.AppNotificationResult.FailureMessage;
        return RedirectToFragment(NewCommentId.ToString());
    }

    public async Task<IActionResult> OnPostDeleteCommentAsync(Guid commentId, CancellationToken token)
    {
        var caseFile = await caseFileService.FindSummaryAsync(Id, token);
        if (caseFile is null || caseFile.IsDeleted) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[EnforcementOperation.DeleteComment]) return BadRequest();

        await caseFileService.DeleteCommentAsync(commentId, token);
        return RedirectToFragment("comments");
    }

    #endregion

    private async Task SetPermissionsAsync() =>
        UserCan = await authorization.SetPermissions(EnforcementOperation.AllOperations, User, CaseFile);

    private PageResult InitializePage(CommentAddDto? newComment = null)
    {
        CommentSection = new CommentsSectionModel
        {
            Comments = CaseFile!.Comments,
            NewComment = newComment ?? new CommentAddDto(Id),
            NewCommentId = NewCommentId,
            NotificationFailureMessage = NotificationFailureMessage,
            CanAddComment = UserCan[EnforcementOperation.AddComment],
            CanDeleteComment = UserCan[EnforcementOperation.DeleteComment],
        };

        CreateEnforcementAction = new EnforcementActionCreateDto();
        IssueEnforcementAction = new MaxDateAndBooleanDto { Option = !CaseFile.MissingData };
        AddEnforcementActionResponse = new MaxDateAndCommentDto();
        ExecuteOrder = new MaxDateOnlyDto();
        AppealOrder = new MaxDateOnlyDto();
        ResolveEnforcementAction = new MaxDateAndBooleanDto { Option = !CaseFile.AttentionNeeded };

        return Page();
    }

    private RedirectToPageResult RedirectToFragment(string? fragment) =>
        RedirectToPage("Details", pageHandler: null, fragment: fragment);
}
