using AirWeb.AppServices.Compliance.Fces;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Report.Compliance.Fce;

public class IndexModel : PageModel
{
    public FceViewDto? Report { get; private set; }
    public IaipDataService.Facilities.Facility? Facility { get; private set; }

    public async Task<ActionResult> OnGetAsync(
        [FromServices] IFceService fceService,
        [FromServices] IFacilityService facilityService,
        [FromRoute] string facilityId,
        [FromRoute] int id)
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

        Facility = await facilityService.FindAsync(airs);
        Report = await fceService.FindAsync(id);
        if (Facility == null || Report == null) return NotFound();

        return Page();
    }
}
