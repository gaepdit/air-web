using AirWeb.AppServices.Core.AuthorizationServices;
using IaipDataService.Facilities;
using System.Text.Json;

namespace AirWeb.WebApp.Pages.Facility;

[Authorize(Policy = nameof(Policies.Staff))]
public class MapModel(IFacilityService service) : PageModel
{
    public IReadOnlyCollection<FacilitySummary> Facilities { get; private set; } = null!;
    public string FacilitiesAsJson => JsonSerializer.Serialize(Facilities);

    [TempData]
    public bool RefreshIaipData { get; set; }

    public async Task<IActionResult> OnGetAsync([FromQuery] bool refresh = false)
    {
        if (refresh)
        {
            RefreshIaipData = true;
            return RedirectToPage();
        }

        Facilities = await service.GetAllAsync(RefreshIaipData);
        return Page();
    }
}
