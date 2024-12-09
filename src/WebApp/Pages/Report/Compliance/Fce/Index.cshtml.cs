using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Fces.SupportingData;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Report.Compliance.Fce;

public class IndexModel : PageModel
{
    public FceViewDto? Report { get; private set; }
    public FceSupportingDataDto SupportingData { get; set; } = default!;
    public IaipDataService.Facilities.Facility? Facility { get; private set; }

    public async Task<ActionResult> OnGetAsync(
        [FromServices] IFceService fceService,
        [FromServices] IFacilityService facilityService,
        [FromRoute] string facilityId,
        [FromRoute] int id,
        CancellationToken token = default)
    {
        FacilityId airs;
        try
        {
            airs = new FacilityId(facilityId);
        }
        catch (ArgumentException)
        {
            return NotFound("Facility ID is invalid.");
        }

        var facilityTask = facilityService.FindAsync(airs);
        var fceTask = fceService.FindAsync(id, token);
        Facility = await facilityTask;
        Report = await fceTask;
        if (Facility == null || Report == null) return NotFound();
        SupportingData = await fceService.GetSupportingDataAsync(id, Report.CompletedDate, token);

        return Page();
    }
}
