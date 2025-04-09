using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using AutoMapper;
using FluentValidation;
using GaEpd.AppLibrary.Extensions;

namespace AirWeb.WebApp.Pages.Enforcement.Edit;

public class ConsentOrderEditModel(
    IEnforcementActionService actionService,
    ICaseFileService caseFileService,
    IValidator<ConsentOrderCommandDto> validator,
    IMapper mapper) : PageModel, ISubmitCancelButtons
{
    [FromRoute]
    public Guid Id { get; set; }

    public string ItemName { get; } = "Consent Order";

    [BindProperty]
    public ConsentOrderCommandDto Item { get; set; } = null!;

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
        if (itemView.ActionType != EnforcementActionType.ConsentOrder)
            return RedirectToPage("Index", new { Id });
        if (!User.CanEdit(itemView)) return Forbid();

        CaseFile = await caseFileService.FindSummaryAsync(itemView.CaseFileId, token);
        if (CaseFile is null) return NotFound();

        Item = mapper.Map<ConsentOrderCommandDto>(itemView);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var itemView = await actionService.FindAsync(Id, token);
        if (itemView is null || !User.CanEdit(itemView) ||
            itemView.ActionType != EnforcementActionType.ConsentOrder)
            return BadRequest();

        await validator.ApplyValidationAsync(Item, ModelState, Id);

        if (!ModelState.IsValid)
        {
            CaseFile = await caseFileService.FindSummaryAsync(itemView.CaseFileId, token);
            return CaseFile is null ? BadRequest() : Page();
        }

        await actionService.UpdateAsync(Id, Item, token);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success,
            $"{itemView.ActionType.GetDescription()} successfully updated.");
        HighlightEnforcementId = Id;
        return RedirectToPage("../Details", pageHandler: null, routeValues: new { Id = itemView.CaseFileId },
            fragment: Id.ToString());
    }
}
