using AirWeb.AppServices.AuthorizationPolicies;
using IaipDataService.Facilities;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.WebApp.Pages.Facility;

[Authorize(Policy = nameof(Policies.Staff))]
public class IndexModel(IFacilityService service) : PageModel
{
    public ReadOnlyDictionary<FacilityId, string> Facilities { get; private set; } = null!;

    [TempData]
    public bool RefreshIaipData { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Enter a facility ID.")]
    [RegularExpression(FacilityId.FacilityIdEnclosedPattern, ErrorMessage = FacilityId.FacilityIdFormatError)]
    public string? Id { get; set; }

    public async Task<IActionResult> OnGetAsync([FromQuery] bool refresh = false)
    {
        if (refresh)
        {
            RefreshIaipData = true;
            return RedirectToPage();
        }

        Facilities = await service.GetListAsync(RefreshIaipData);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Facilities = await service.GetListAsync(RefreshIaipData);
            return Page();
        }

        if (Id == null || !FacilityId.IsValidFormat(Id))
            ModelState.AddModelError(nameof(Id), FacilityId.FacilityIdFormatError);
        else if (!await service.ExistsAsync((FacilityId)Id))
            ModelState.AddModelError(nameof(Id),
                "A Facility with that ID does not exist or has not been approved in the IAIP.");

        if (ModelState.IsValid)
            return RedirectToPage("Details", routeValues: new { id = Id });

        Facilities = await service.GetListAsync(RefreshIaipData);
        return Page();
    }
}
