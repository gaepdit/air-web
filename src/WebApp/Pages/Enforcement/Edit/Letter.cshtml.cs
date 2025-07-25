﻿using AirWeb.AppServices.DtoInterfaces;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.WebApp.Models;

namespace AirWeb.WebApp.Pages.Enforcement.Edit;

public class LetterEditModel(
    IEnforcementActionService actionService,
    ICaseFileService caseFileService)
    : PageModel, ISubmitCancelButtons
{
    [FromRoute]
    public Guid Id { get; set; }

    [BindProperty]
    public EnforcementActionEditDto Item { get; set; } = null!;

    public bool ShowResponseRequested { get; private set; }
    public string ItemName { get; private set; } = null!;
    public CaseFileSummaryDto? CaseFile { get; set; }

    // Form buttons
    public string SubmitText => "Save Changes";
    public string CancelRoute => "../Details";
    public string RouteId => CaseFile?.Id.ToString() ?? string.Empty;

    [TempData]
    public Guid? HighlightEnforcementId { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        var itemView = await actionService.FindAsync(Id, token);
        if (itemView is null) return NotFound();
        if (!User.CanEdit(itemView)) return Forbid();

        CaseFile = await caseFileService.FindSummaryAsync(itemView.CaseFileId, token);
        if (CaseFile is null) return NotFound();
        if (!User.CanEditCaseFile(CaseFile)) return Forbid();

        Item = new EnforcementActionEditDto
        {
            Comment = itemView.Notes,
            IssueDate = itemView.IssueDate,
        };

        if (itemView is IResponseRequested responseRequested)
        {
            Item.ResponseRequested = responseRequested.ResponseRequested;
            ShowResponseRequested = true;
        }

        ItemName = itemView.ActionType.GetDisplayName();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var itemView = await actionService.FindAsync(Id, token);
        if (itemView is null || !User.CanEdit(itemView)) return BadRequest();

        CaseFile = await caseFileService.FindSummaryAsync(itemView.CaseFileId, token);
        if (CaseFile is null || !User.CanEditCaseFile(CaseFile)) return BadRequest();

        await actionService.UpdateAsync(Id, Item, token);

        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success,
            $"{itemView.ActionType.GetDisplayName()} successfully updated.");
        HighlightEnforcementId = Id;

        return RedirectToPage("../Details", pageHandler: null, routeValues: new { Id = itemView.CaseFileId },
            fragment: Id.ToString());
    }
}
