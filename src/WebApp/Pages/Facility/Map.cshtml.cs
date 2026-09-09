using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.Domain.Core.Data;
using AirWeb.WebApp.Platform.Settings;
using IaipDataService.Facilities;
using System.Text.Json;

namespace AirWeb.WebApp.Pages.Facility;

[Authorize(Policy = nameof(Policies.Staff))]
public class MapModel(IFacilityService service) : PageModel
{
    public IReadOnlyCollection<FacilitySummary> Facilities { get; private set; } = null!;

    public string FacilitiesAsJson => JsonSerializer.Serialize(Facilities, SerializationDefaults.Options);

    public static List<CityLocation> CityLocations => CityLocationData.GetAll;

    [TempData]
    public bool RefreshIaipData { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken token = default)
    {
        Facilities = await service.GetAllAsync(RefreshIaipData, includePortableSources: false, token);
        return Page();
    }

    public IActionResult OnPostRefreshIaipAsync()
    {
        RefreshIaipData = true;
        return RedirectToPage();
    }
}
