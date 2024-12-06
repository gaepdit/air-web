using AirWeb.Domain.Reports;
using AirWeb.Domain.Reports.Compliance;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Report.Compliance.Fce;

public class IndexModel : PageModel
{
    public FceReport? Report { get; set; }

    public async Task<ActionResult> OnGetAsync(
        [FromServices] IReportsRepository repository,
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

        Report = await repository.GetFceReportAsync(airs, id);
        if (Report?.Facility is null) return NotFound();
        if (Report.Facility.RegulatoryData is null) return NotFound();

        return Page();
    }
}
