using AirWeb.AppServices.Compliance.Enforcement;
using AirWeb.AppServices.Compliance.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Compliance.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Compliance.Enforcement.Permissions;
using AutoMapper;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Enforcement.Add;

public class AdministrativeOrderAddModel(
    IEnforcementActionService actionService,
    ICaseFileService caseFileService,
    IValidator<AdministrativeOrderCommandDto> validator,
    IMapper mapper) : PageModel, ISubmitCancelButtons
{
    [FromRoute]
    public int Id { get; set; }

    public string ItemName { get; } = "Administrative Order";

    [BindProperty]
    public AdministrativeOrderCommandDto Item { get; set; } = null!;

    public CaseFileSummaryDto? CaseFile { get; set; }

    // Form buttons
    public string SubmitText => $"Add {ItemName}";
    public string CancelRoute => "../Details";
    public string RouteId => CaseFile?.Id.ToString() ?? string.Empty;

    [TempData]
    public Guid? HighlightEnforcementId { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        CaseFile = await caseFileService.FindSummaryAsync(Id, token);
        if (CaseFile is null) return NotFound();
        if (!User.CanEditCaseFile(CaseFile)) return Forbid();

        Item = new AdministrativeOrderCommandDto();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var caseFile = await caseFileService.FindDetailedAsync(Id, token);
        if (caseFile is null || !User.CanEditCaseFile(caseFile)) return BadRequest();

        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            CaseFile = mapper.Map<CaseFileSummaryDto>(caseFile);
            return Page();
        }

        HighlightEnforcementId = await actionService.CreateAsync(Id, Item, token);

        return caseFile.MissingPollutantsOrPrograms || caseFile.MissingViolationType
            ? RedirectToPage("../PollutantsPrograms", new { Id })
            : RedirectToPage("../Details", pageHandler: null, routeValues: new { Id },
                fragment: HighlightEnforcementId.ToString());
    }
}
