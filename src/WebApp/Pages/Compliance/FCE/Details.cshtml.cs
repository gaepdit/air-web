using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.AppServices.Enforcement.Search;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.WebApp.Models;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using IaipDataService.PermitFees;

namespace AirWeb.WebApp.Pages.Compliance.FCE;

[Authorize(Policy = nameof(Policies.Staff))]
public class DetailsModel(
    IFceService fceService,
    IWorkEntrySearchService workEntrySearchService,
    ICaseFileSearchService caseFileSearchService,
    IPermitFeesService permitFeesService,
    IAuthorizationService authorization) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    public FceViewDto? Item { get; private set; }
    public IPaginatedResult<WorkEntrySearchResultDto> ComplianceSummary { get; private set; } = null!;
    public IPaginatedResult<CaseFileSearchResultDto> CaseFileSummary { get; private set; } = null!;
    public List<AnnualFeeSummary> AnnualFeesSummary { get; private set; } = null!;
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

        Item = await fceService.FindAsync(Id, token);
        if (Item is null) return NotFound();

        await SetPermissionsAsync();
        if (Item.IsDeleted && !UserCan[ComplianceOperation.ViewDeleted]) return NotFound();

        await LoadSupportingData(token);
        CommentSection = new CommentsSectionModel
        {
            Comments = Item.Comments,
            NewComment = new CommentAddDto(Id),
            NewCommentId = NewCommentId,
            NotificationFailureMessage = NotificationFailureMessage,
            CanAddComment = UserCan[ComplianceOperation.AddComment],
            CanDeleteComment = UserCan[ComplianceOperation.DeleteComment],
        };
        return Page();
    }

    public async Task<IActionResult> OnPostNewCommentAsync(CommentAddDto newComment,
        CancellationToken token)
    {
        Item = await fceService.FindAsync(Id, token);
        if (Item is null || Item.IsDeleted) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[ComplianceOperation.AddComment]) return BadRequest();

        if (!ModelState.IsValid)
        {
            await LoadSupportingData(token);
            CommentSection = new CommentsSectionModel
            {
                Comments = Item.Comments,
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
        Item = await fceService.FindAsync(Id, token);
        if (Item is null || Item.IsDeleted) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[ComplianceOperation.DeleteComment]) return BadRequest();

        await fceService.DeleteCommentAsync(commentId, token);
        return RedirectToPage("Details", pageHandler: null, fragment: "comments");
    }

    private async Task LoadSupportingData(CancellationToken token)
    {
        await LoadComplianceData(token);
        await LoadEnforcementData(token);
        await LoadAnnualFeesData();
    }

    private async Task LoadComplianceData(CancellationToken token)
    {
        var spec = new WorkEntrySearchDto
        {
            PartialFacilityId = Item!.FacilityId,
            EventDateTo = Item.CompletedDate,
            EventDateFrom = Item.SupportingDataStartDate,
        };
        var paging = new PaginatedRequest(pageNumber: 1, pageSize: 100,
            sorting: WorkEntrySortBy.WorkTypeAsc.GetDescription());
        ComplianceSummary = await workEntrySearchService.SearchAsync(spec, paging, token: token);
    }

    private async Task LoadEnforcementData(CancellationToken token)
    {
        var spec = new CaseFileSearchDto
        {
            PartialFacilityId = Item!.FacilityId,
            EnforcementDateTo = Item.CompletedDate,
            EnforcementDateFrom = Item.ExtendedDataStartDate,
        };
        var paging = new PaginatedRequest(pageNumber: 1, pageSize: 100,
            sorting: CaseFileSortBy.IdAsc.GetDescription());
        CaseFileSummary = await caseFileSearchService.SearchAsync(spec, paging, token: token);
    }

    private async Task LoadAnnualFeesData()
    {
        AnnualFeesSummary = await permitFeesService.GetAnnualFeesHistoryAsync((FacilityId)Item!.FacilityId,
            Item.CompletedDate, Fce.ExtendedDataPeriod, RefreshIaipData);
    }

    private async Task SetPermissionsAsync() =>
        UserCan = await authorization.SetPermissions(ComplianceOperation.AllOperations, User, Item);
}
