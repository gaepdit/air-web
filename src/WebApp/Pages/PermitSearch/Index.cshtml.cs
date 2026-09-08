using AirWeb.WebApp.Platform.Settings;
using IaipDataService.Facilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace AirWeb.WebApp.Pages.PermitSearch;

public class PermitSearchIndex(IFacilityService service) : PageModel
{
    [Display(Name = "Facility ID/AIRS Number")]
    public string? Id { get; set; }

    [Display(Name = "Facility Name")]
    public string? Name { get; set; }

    public string FacilitiesAsJson => JsonSerializer.Serialize(Facilities, SerializationDefaults.Options);
    public IReadOnlyCollection<FacilityList> Facilities { get; private set; } = null!;

    public async Task OnGetAsync(CancellationToken token = default) =>
        Facilities = await service.GetListAsync(token);
}
