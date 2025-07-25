﻿using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.WebApp.Models;
using AutoMapper;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Enforcement.Edit;

public class AdministrativeOrderEditModel(
    IEnforcementActionService actionService,
    ICaseFileService caseFileService,
    IValidator<AdministrativeOrderCommandDto> validator,
    IMapper mapper) : PageModel, ISubmitCancelButtons
{
    [FromRoute]
    public Guid Id { get; set; }

    [BindProperty]
    public AdministrativeOrderCommandDto Item { get; set; } = null!;

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
        if (itemView.ActionType != EnforcementActionType.AdministrativeOrder)
            return RedirectToPage("Index", new { Id });
        if (!User.CanEdit(itemView)) return Forbid();

        CaseFile = await caseFileService.FindSummaryAsync(itemView.CaseFileId, token);
        if (CaseFile is null) return NotFound();
        if (!User.CanEditCaseFile(CaseFile)) return Forbid();

        Item = mapper.Map<AdministrativeOrderCommandDto>(itemView);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var itemView = await actionService.FindAsync(Id, token);
        if (itemView is null || !User.CanEdit(itemView) ||
            itemView.ActionType != EnforcementActionType.AdministrativeOrder)
            return BadRequest();

        CaseFile = await caseFileService.FindSummaryAsync(itemView.CaseFileId, token);
        if (CaseFile is null || !User.CanEditCaseFile(CaseFile)) return BadRequest();

        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
            return Page();

        await actionService.UpdateAsync(Id, Item, token);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success,
            $"{itemView.ActionType.GetDisplayName()} successfully updated.");
        HighlightEnforcementId = Id;
        return RedirectToPage("../Details", pageHandler: null, routeValues: new { Id = itemView.CaseFileId },
            fragment: Id.ToString());
    }
}
