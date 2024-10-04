using AirWeb.AppServices.Permissions;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Facility;

[Authorize(Policy = nameof(Policies.Staff))]
public class DetailsModel(IFacilityService service) : PageModel
{
    [FromRoute]
    public string? FacilityId { get; set; }

    public IaipDataService.Facilities.Facility? Facility { get; protected set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (FacilityId is null) return NotFound("Facility ID not found.");
        Facility = await service.FindAsync((FacilityId)FacilityId);
        if (Facility is null) return NotFound("Facility ID not found.");
        return Page();
    }
}
