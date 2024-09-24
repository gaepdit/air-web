using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Permissions;

namespace AirWeb.WebApp.Pages.Facility;

[Authorize(Policy = nameof(Policies.Staff))]
public class DetailsModel(IFacilityService service) : PageModel
{
    [FromRoute]
    public string? FacilityId { get; set; }

    public FacilityViewDto? Facility { get; protected set; }
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        Facility = await service.FindAsync(FacilityId);
        if (Facility is null) return NotFound("Facility ID not found.");
        return Page();
    }
}
