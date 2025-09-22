using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Enforcement.EnforcementActionQuery;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.WebApp.Models;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Enforcement;

public class StipulatedPenaltiesEditModel(
    IEnforcementActionService actionService,
    ICaseFileService caseFileService,
    IValidator<StipulatedPenaltyAddDto> validator) : PageModel, ISubmitCancelButtons
{
    [FromRoute]
    public Guid Id { get; set; }

    [BindProperty]
    public StipulatedPenaltyAddDto NewPenalty { get; set; } = null!;

    public CaseFileSummaryDto? CaseFile { get; set; }

    public CoViewDto ConsentOrder { get; set; } = null!;

    // Form buttons
    public string SubmitText => "Add New";
    public string CancelRoute => "Details";
    public string RouteId => CaseFile?.Id.ToString() ?? string.Empty;

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        if (Id == Guid.Empty) return RedirectToPage("Index");

        var coView = await actionService.FindConsentOrderAsync(Id, token);
        if (coView is null) return NotFound();
        if (!User.CanEditStipulatedPenalties(coView)) return Forbid();

        CaseFile = await caseFileService.FindSummaryAsync(coView.CaseFileId, token);
        if (CaseFile is null) return BadRequest();
        if (!User.CanEditCaseFile(CaseFile)) return Forbid();

        ConsentOrder = coView;
        NewPenalty = new StipulatedPenaltyAddDto();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var coView = await actionService.FindConsentOrderAsync(Id, token);
        if (coView is null || !User.CanEditStipulatedPenalties(coView)) return BadRequest();

        CaseFile = await caseFileService.FindSummaryAsync(coView.CaseFileId, token);
        if (CaseFile is null || !User.CanEditCaseFile(CaseFile)) return BadRequest();

        await validator.ApplyValidationAsync(NewPenalty, ModelState, coView.Id);
        if (!ModelState.IsValid)
        {
            ConsentOrder = coView;
            return Page();
        }

        await actionService.AddStipulatedPenalty(Id, NewPenalty, token);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "Stipulated penalty added.");
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeletePenaltyAsync(Guid penaltyId, CancellationToken token)
    {
        var coView = await actionService.FindConsentOrderAsync(Id, token);
        if (coView is null || !User.CanEditStipulatedPenalties(coView)) return BadRequest();

        CaseFile = await caseFileService.FindSummaryAsync(coView.CaseFileId, token);
        if (CaseFile is null || !User.CanEditCaseFile(CaseFile)) return BadRequest();

        await actionService.DeletedStipulatedPenalty(coView.Id, penaltyId, token);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "Stipulated penalty deleted.");
        return RedirectToPage();
    }
}
