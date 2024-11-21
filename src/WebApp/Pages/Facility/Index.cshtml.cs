using AirWeb.AppServices.Permissions;
using IaipDataService.Facilities;
using System.Collections.ObjectModel;

namespace AirWeb.WebApp.Pages.Facility;

[Authorize(Policy = nameof(Policies.Staff))]
public class IndexModel(IFacilityService service) : PageModel
{
    public ReadOnlyDictionary<FacilityId, string> Facilities { get; private set; } = null!;
    public async Task OnGetAsync() => Facilities = await service.GetListAsync();
}
