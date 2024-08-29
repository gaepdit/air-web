﻿using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Compliance;
using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Permissions;

namespace AirWeb.WebApp.Pages.Compliance.FCE;

[Authorize(Policy = nameof(Policies.Staff))]
public class DetailsModel(IFceService fceService, IAuthorizationService authorization) : PageModel
{
    public FceViewDto Item { get; private set; } = default!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null) return RedirectToPage("Index");
        var item = await fceService.FindAsync(id.Value);
        if (item is null) return NotFound();

        await SetPermissionsAsync(item);
        if (item.IsDeleted && !UserCan[ComplianceWorkOperation.ManageDeletions]) return NotFound();

        Item = item;
        return Page();
    }

    [TempData]
    public Guid NewCommentId { get; set; }

    [TempData]
    public string? CommentNotificationFailureMessage { get; set; }

    public async Task<IActionResult> OnPostNewCommentAsync(int? id, CommentAddDto<int> newComment,
        CancellationToken token)
    {
        if (id is null) return BadRequest();
        //
        // var complaintView = await complaintService.FindAsync(id.Value, includeDeletedActions: true, token);
        // if (complaintView is null || complaintView.IsDeleted) return BadRequest();
        //
        // await SetPermissionsAsync(complaintView);
        // if (!UserCan[ComplaintOperation.EditActions]) return BadRequest();
        //
        // if (!ModelState.IsValid)
        // {
        //     ValidatingSection = nameof(OnPostNewActionAsync);
        //     ComplaintView = complaintView;
        //     await PopulateSelectListsAsync();
        //     return Page();
        // }

        var addCommentResult = await fceService.AddCommentAsync(id.Value, newComment, token);
        NewCommentId = addCommentResult.CommentId;
        if (!addCommentResult.AppNotificationResult.Success)
            CommentNotificationFailureMessage = addCommentResult.AppNotificationResult.FailureMessage;
        return RedirectToPage("Details", pageHandler: null, routeValues: new { id }, fragment: NewCommentId.ToString());
    }

    private async Task SetPermissionsAsync(FceViewDto item)
    {
        foreach (var operation in ComplianceWorkOperation.AllOperations)
            UserCan[operation] = (await authorization.AuthorizeAsync(User, item, operation)).Succeeded;
    }
}