using AirWeb.AppServices.Permissions;
using AirWeb.WebApp.Platform.Constants;
using IaipDataService.Facilities;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace AirWeb.WebApp.Pages.Facility;

[Authorize(Policy = nameof(Policies.Staff))]
public class SourceTestsModel(IFacilityService facilityService, ISourceTestService sourceTestService) : PageModel
{
    [FromRoute]
    public string? FacilityId { get; set; }

    public IaipDataService.Facilities.Facility? Facility { get; private set; }
    public IList<SourceTestSummary> SourceTests { get; private set; } = [];

    [TempData]
    public bool RefreshIaipData { get; set; }

    public async Task<IActionResult> OnGetAsync([FromQuery] bool refresh = false)
    {
        if (refresh)
        {
            RefreshIaipData = true;
            return RedirectToPage();
        }

        if (FacilityId is null) return NotFound("Facility ID not found.");
        Facility = await facilityService.FindAsync((FacilityId)FacilityId, RefreshIaipData);
        if (Facility is null) return NotFound("Facility ID not found.");

        SourceTests = (await sourceTestService.GetSourceTestsForFacilityAsync((FacilityId)FacilityId, RefreshIaipData))
            .Take(GlobalConstants.PageSize).ToList();

        return Page();
    }
}
