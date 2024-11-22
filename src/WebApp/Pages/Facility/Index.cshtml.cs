using AirWeb.AppServices.Permissions;
using IaipDataService.Facilities;
using System.Collections.ObjectModel;

namespace AirWeb.WebApp.Pages.Facility;

[Authorize(Policy = nameof(Policies.Staff))]
public class IndexModel(IFacilityService service) : PageModel
{
    public ReadOnlyDictionary<FacilityId, string> Facilities { get; private set; } = null!;

    [TempData]
    public bool RefreshIaipData { get; set; }

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
}
