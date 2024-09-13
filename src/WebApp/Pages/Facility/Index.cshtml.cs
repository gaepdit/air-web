using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Permissions;

namespace AirWeb.WebApp.Pages.Facility;

[Authorize(Policy = nameof(Policies.Staff))]
public class IndexModel(IFacilityService service) : PageModel
{
    public IReadOnlyCollection<FacilityViewDto> Facilities { get; private set; } = null!;
    public async Task OnGetAsync() => Facilities = await service.GetListAsync();
}
