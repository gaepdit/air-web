using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.Accs;
using AirWeb.AppServices.Utilities;
using AirWeb.WebApp.Platform.PrintoutModels;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Print.ACC;

public class IndexModel : PageModel
{
    public AccViewDto? Report { get; set; }
    public IaipDataService.Facilities.Facility? Facility { get; private set; }
    public MemoHeader MemoHeader { get; private set; }

    public async Task<ActionResult> OnGetAsync(
        [FromServices] IWorkEntryService workEntryService,
        [FromServices] IFacilityService facilityService,
        [FromRoute] int id)
    {
        Report = await workEntryService.FindAsync(id, false) as AccViewDto;
        if (Report == null || Report.IsDeleted) return NotFound();

        Facility = await facilityService.FindFacilityDetailsAsync((FacilityId)Report.FacilityId);
        if (Facility == null) return NotFound();

        MemoHeader = new MemoHeader
        {
            MemoDate = Report.ClosedDate,
            From = Report.ResponsibleStaff?.Name,
            Subject = (Report.AccReportingYear is null
                          ? $"Title V Annual Certification received {Report.ReceivedDate.ToString(DateTimeFormats.ShortDate)}"
                          : $"Title V Annual Certification for {Report.AccReportingYear}") + Environment.NewLine +
                      $"{Facility.Name}, {Facility.FacilityAddress?.City}" + Environment.NewLine +
                      $"AIRS # {Facility.Id}",
        };

        return Page();
    }
}
