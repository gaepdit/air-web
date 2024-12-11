﻿using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.WebApp.Models;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Pages.Compliance.FCE;

[Authorize(Policy = nameof(Policies.Staff))]
public class DetailsModel(
    IFceService fceService,
    IComplianceSearchService searchService,
    IAuthorizationService authorization) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    public FceViewDto? Item { get; private set; }
    public IPaginatedResult<WorkEntrySearchResultDto> SearchResults { get; private set; } = default!;
    public CommentsSectionModel CommentSection { get; set; } = null!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    [TempData]
    public Guid NewCommentId { get; set; }

    [TempData]
    public string? NotificationFailureMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken token = default)
    {
        if (Id == 0) return RedirectToPage("Index");
        Item = await fceService.FindAsync(Id, token);
        if (Item is null) return NotFound();

        await SetPermissionsAsync();
        if (Item.IsDeleted && !UserCan[ComplianceWorkOperation.ViewDeleted]) return NotFound();

        await LoadSupportingData(token);
        CommentSection = new CommentsSectionModel
        {
            Comments = Item.Comments,
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
        Item = await fceService.FindAsync(Id, token);
        if (Item is null || Item.IsDeleted) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[ComplianceWorkOperation.AddComment]) return BadRequest();

        if (!ModelState.IsValid)
        {
            await LoadSupportingData(token);
            CommentSection = new CommentsSectionModel
            {
                Comments = Item.Comments,
                NewComment = newComment,
                CanAddComment = UserCan[ComplianceWorkOperation.AddComment],
                CanDeleteComment = UserCan[ComplianceWorkOperation.DeleteComment],
            };
            return Page();
        }

        var addCommentResult = await fceService.AddCommentAsync(Id, newComment, token);
        NewCommentId = addCommentResult.Id;
        if (addCommentResult.AppNotificationResult is { Success: false })
            NotificationFailureMessage = addCommentResult.AppNotificationResult.FailureMessage;
        return RedirectToPage("Details", pageHandler: null, routeValues: new { Id }, fragment: NewCommentId.ToString());
    }

    public async Task<IActionResult> OnPostDeleteCommentAsync(Guid commentId, CancellationToken token)
    {
        Item = await fceService.FindAsync(Id, token);
        if (Item is null || Item.IsDeleted) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[ComplianceWorkOperation.DeleteComment]) return BadRequest();

        await fceService.DeleteCommentAsync(commentId, token);
        return RedirectToPage("Details", pageHandler: null, routeValues: new { Id }, fragment: "comments");
    }

    private async Task LoadSupportingData(CancellationToken token)
    {
        var spec = new WorkEntrySearchDto
        {
            PartialFacilityId = Item!.FacilityId,
            EventDateTo = Item.CompletedDate,
            EventDateFrom = Item.SupportingDataStartDate,
        };
        var paging = new PaginatedRequest(pageNumber: 1, pageSize: 100, sorting: SortBy.WorkTypeAsc.GetDescription());
        SearchResults = await searchService.SearchWorkEntriesAsync(spec, paging, token: token);
    }

    private async Task SetPermissionsAsync() =>
        UserCan = await authorization.SetPermissions(ComplianceWorkOperation.AllOperations, User, Item);
}
