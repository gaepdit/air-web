using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Print.Fce;

public class IndexModel : PageModel
{
    public FceViewDto? Report { get; private set; }
    public WorkEntryDataSummary SupportingData { get; set; } = null!;
    public IaipDataService.Facilities.Facility? Facility { get; private set; }

    public async Task<ActionResult> OnGetAsync(
        [FromServices] IFceService fceService,
        [FromServices] IFacilityService facilityService,
        [FromServices] IWorkEntryService workEntryService,
        [FromRoute] int id,
        CancellationToken token = default)
    {
        Report = await fceService.FindAsync(id, token);
        if (Report == null || Report.IsDeleted) return NotFound();

        Facility = await facilityService.FindFacilityDetailsAsync((FacilityId?)Report!.FacilityId);
        if (Facility == null) return NotFound();

        SupportingData = await workEntryService.GetDataSummaryAsync(Facility.Id, token);

        return Page();
    }
}
