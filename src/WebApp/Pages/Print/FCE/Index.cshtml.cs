using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Fces.SupportingData;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Print.FCE;

public class IndexModel : PageModel
{
    public FceViewDto? FceView { get; private set; }
    public SupportingDataSummary SupportingData { get; set; } = null!;
    public IaipDataService.Facilities.Facility? Facility { get; private set; }

    public async Task<ActionResult> OnGetAsync(
        [FromServices] IFceService fceService,
        [FromServices] IFacilityService facilityService,
        [FromRoute] int id,
        CancellationToken token = default)
    {
        FceView = await fceService.FindAsync(id, token);
        if (FceView == null || FceView.IsDeleted) return NotFound();

        Facility = await facilityService.FindFacilityDetailsAsync((FacilityId)FceView.FacilityId);
        if (Facility == null) return NotFound();

        SupportingData = await fceService.GetSupportingDataAsync(Facility.Id, FceView.CompletedDate, token);

        return Page();
    }
}
