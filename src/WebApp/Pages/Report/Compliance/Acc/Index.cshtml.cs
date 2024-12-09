using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.Accs;
using AirWeb.WebApp.Platform.ReportsModels;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Report.Compliance.Acc;

public class IndexModel : PageModel
{
    public AccViewDto? Report { get; set; }
    public IaipDataService.Facilities.Facility? Facility { get; private set; }
    public MemoHeader MemoHeader { get; private set; }

    public async Task<ActionResult> OnGetAsync(
        [FromServices] IWorkEntryService workEntryService,
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

        var facilityTask = facilityService.FindAsync(airs);
        var entryTask = workEntryService.FindAsync(id);
        Facility = await facilityTask;
        Report = await entryTask as AccViewDto;
        if (Facility == null || Report == null) return NotFound();

        MemoHeader = new MemoHeader
        {
            Date = Report.ClosedDate,
            From = Report.ResponsibleStaff?.DisplayName,
            Subject = $"Title V Annual Certification for {Report.AccReportingYear}" + Environment.NewLine +
                      $"{Facility.Name}, {Facility.FacilityAddress?.City}" + Environment.NewLine +
                      $"AIRS # {Facility.Id}",
        };

        return Page();
    }
}
