﻿using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using GaEpd.AppLibrary.Extensions;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.WebApp.Pages.Enforcement.Edit;

public class LetterEditModel(
    IEnforcementActionService actionService,
    ICaseFileService caseFileService)
    : PageModel, ISubmitCancelButtons
{
    [FromRoute]
    public Guid Id { get; set; }

    [BindProperty]
    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string? Comment { get; set; }

    [BindProperty]
    [Display(Name = "Response requested")]
    public bool ResponseRequested { get; set; }

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

        Comment = itemView.Notes;
        if (itemView is IResponseRequested responseRequested)
        {
            ResponseRequested = responseRequested.ResponseRequested;
            ShowResponseRequested = true;
        }

        ItemName = itemView.ActionType.GetDescription();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var itemView = await actionService.FindAsync(Id, token);
        if (itemView is null || !User.CanEdit(itemView)) return BadRequest();

        await actionService.UpdateAsync(Id, Comment, ResponseRequested, token);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success,
            $"{itemView.ActionType.GetDescription()} successfully updated.");
        HighlightEnforcementId = Id;
        return RedirectToPage("../Details", pageHandler: null, routeValues: new { Id = itemView.CaseFileId },
            fragment: Id.ToString());
    }
}
