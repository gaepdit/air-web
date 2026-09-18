using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.Compliance.Fces;
using AirWeb.AppServices.Compliance.Enforcement;
using AirWeb.Domain.Compliance.DataExchange;
using IaipDataService.Facilities;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.WebApp.Pages.Compliance;

public class EpaIdModel(
    IFacilityService facilityService,
    IComplianceWorkService workService,
    IFceService fceService,
    ICaseFileService caseFileService,
    IEnforcementActionService enforcementActionService) : PageModel
{
    [BindProperty, Required(ErrorMessage = "Enter an Identifier.")]
    public string? EpaId { get; set; }

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPostAsync(CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(EpaId)) return Page();

        if (FacilityId.IsValidFormat(EpaId))
        {
            if (await facilityService.ExistsAsync((FacilityId)EpaId))
                return RedirectToPage("/Facility/Details", new { id = EpaId });

            ModelState.AddModelError(nameof(EpaId), "A Facility with that ID could not be found.");
            return Page();
        }

        if (!EpaActivityId.IsValidFormat(EpaId))
        {
            ModelState.AddModelError(nameof(EpaId), "Invalid ID format.");
            return Page();
        }

        var epaActivityId = new EpaActivityId(EpaId);

        var id = await workService.LookUpEpaIdAsync(epaActivityId, token);
        if (id > 0) return RedirectToPage("/Compliance/Work/Details", new { id });

        id = await fceService.LookUpEpaIdAsync(epaActivityId, token);
        if (id > 0) return RedirectToPage("/Compliance/FCE/Details", new { id });

        id = await caseFileService.LookUpEpaIdAsync(epaActivityId, token);
        if (id > 0) return RedirectToPage("/Enforcement/Details", new { id });

        id = await enforcementActionService.LookUpEpaIdAsync(epaActivityId, token);
        if (id > 0) return RedirectToPage("/Enforcement/Details", new { id });

        ModelState.AddModelError(nameof(EpaId), "An entry with that ID could not be found.");
        return Page();
    }
}
