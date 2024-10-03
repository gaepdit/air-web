using AirWeb.AppServices.Permissions;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Facility;

[Authorize(Policy = nameof(Policies.Staff))]
public class IndexModel(IFacilityService service) : PageModel
{
    public IReadOnlyCollection<IaipDataService.Facilities.Facility> Facilities { get; private set; } = null!;
    public async Task OnGetAsync() => Facilities = await service.GetListAsync();
}
