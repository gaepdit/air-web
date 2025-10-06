﻿using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Compliance.ComplianceWork;
using AirWeb.AppServices.Compliance.ComplianceWork.SourceTestReviews;
using AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Query;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using AirWeb.WebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.WebApp.Pages.Compliance.Work;

[Authorize(Policy = nameof(Policies.Staff))]
public class DetailsModel(IWorkEntryService entryService, IAuthorizationService authorization) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    public IWorkEntryViewDto? Item { get; private set; }
    public CommentsSectionModel CommentSection { get; set; } = null!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    [Display(Name = "Linked Enforcement Cases")]
    public IEnumerable<int> CaseFileIds { get; private set; } = [];

    [TempData]
    public Guid NewCommentId { get; set; }

    [TempData]
    public string? NotificationFailureMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken token = default)
    {
        if (Id == 0) return RedirectToPage("Index");
        Item = await entryService.FindAsync(Id, true, token);
        if (Item is null) return NotFound();

        await SetPermissionsAsync();
        if (Item.IsDeleted && !UserCan[ComplianceOperation.ViewDeleted]) return NotFound();

        if (Item.WorkEntryType == WorkEntryType.SourceTestReview && !Item.IsDeleted)
            return RedirectToPage("../SourceTest/Details", new { ((SourceTestReviewViewDto)Item).ReferenceNumber });

        if (Item.IsComplianceEvent)
            CaseFileIds = await entryService.GetCaseFileIdsAsync(Id, token);

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
        Item = await entryService.FindAsync(Id, true, token);
        if (Item is null || Item.IsDeleted) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[ComplianceOperation.AddComment]) return BadRequest();

        if (!ModelState.IsValid)
        {
            CaseFileIds = await entryService.GetCaseFileIdsAsync(Id, token);

            CommentSection = new CommentsSectionModel
            {
                Comments = Item.Comments,
                NewComment = newComment,
                CanAddComment = UserCan[ComplianceOperation.AddComment],
                CanDeleteComment = UserCan[ComplianceOperation.DeleteComment],
            };

            return Page();
        }

        var result = await entryService.AddCommentAsync(Id, newComment, token);
        NewCommentId = result.Id;
        if (result.HasWarning) TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, result.WarningMessage);
        return RedirectToPage("Details", pageHandler: null, fragment: NewCommentId.ToString());
    }

    public async Task<IActionResult> OnPostDeleteCommentAsync(Guid commentId, CancellationToken token)
    {
        Item = await entryService.FindAsync(Id, true, token);
        if (Item is null || Item.IsDeleted) return BadRequest();

        await SetPermissionsAsync();
        if (!UserCan[ComplianceOperation.DeleteComment]) return BadRequest();

        await entryService.DeleteCommentAsync(commentId, token);
        return RedirectToPage("Details", pageHandler: null, fragment: "comments");
    }

    private async Task SetPermissionsAsync() =>
        UserCan = await authorization.SetPermissions(ComplianceOperation.AllOperations, User, Item);
}
