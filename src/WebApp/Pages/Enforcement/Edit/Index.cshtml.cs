using AirWeb.AppServices.Enforcement;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;

namespace AirWeb.WebApp.Pages.Enforcement.Edit;

public class EditEnforcementActionRedirectModel : PageModel
{
    [FromRoute]
    public Guid Id { get; set; }

    public async Task<IActionResult> OnGetAsync([FromServices] IEnforcementActionService service,
        CancellationToken token)
    {
        var actionType = await service.GetEnforcementActionType(Id, token);
        if (actionType is null) return NotFound();
        return RedirectToPage(actionType switch
        {
            EnforcementActionType.AdministrativeOrder => "AdministrativeOrder",
            EnforcementActionType.ConsentOrder => "ConsentOrder",
            EnforcementActionType.InformationalLetter
                or EnforcementActionType.LetterOfNoncompliance
                or EnforcementActionType.NoFurtherActionLetter
                or EnforcementActionType.NovNfaLetter
                or EnforcementActionType.NoticeOfViolation
                or EnforcementActionType.ProposedConsentOrder => "Letter",
            _ => "../Index",
        }, routeValues: new { Id });
    }
}
