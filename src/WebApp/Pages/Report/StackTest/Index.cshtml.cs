using AirWeb.AppServices.Reports;
using AirWeb.AppServices.Reports.StackTestDto;
using AirWeb.Domain.Identity;
using AirWeb.WebApp.Platform.ReportsModels;
using AirWeb.WebApp.Platform.Settings;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Report.StackTest;

public class IndexModel : PageModel
{
    public BaseStackTestReport? Report { get; private set; }
    public MemoHeader MemoHeader { get; private set; }
    public OrganizationInfo OrganizationInfo { get; private set; } = default!;
    public bool ShowConfidentialWarning { get; private set; }

    public async Task<ActionResult> OnGetAsync(
        [FromServices] IReportsService service,
        [FromRoute] string facilityId,
        [FromRoute] int referenceNumber,
        [FromQuery] bool includeConfidentialInfo = false)
    {
        if (includeConfidentialInfo)
        {
            if (User.Identity is not { IsAuthenticated: true }) return Challenge();
            if (User.Identity.Name is null || !User.Identity.Name.IsValidEmailDomain()) return Forbid();
        }

        FacilityId airs;
        try
        {
            airs = new FacilityId(facilityId);
        }
        catch (ArgumentException)
        {
            return NotFound("Facility ID is invalid.");
        }

        var report = await service.GetStackTestReportAsync(airs, referenceNumber);
        if (report?.Facility is null) return NotFound();

        Report = includeConfidentialInfo ? report : report.RedactedStackTestReport();
        MemoHeader = new MemoHeader
        {
            To = Report.ComplianceManager.DisplayName,
            From = Report.ReviewedByStaff.DisplayName,
            Through = Report.TestingUnitManager.DisplayName,
            Subject = Report.ReportTypeSubject.ToUpperInvariant(),
        };
        ShowConfidentialWarning = includeConfidentialInfo && Report.ConfidentialParameters.Count != 0;
        OrganizationInfo = AppSettings.OrganizationInfo with { NameOfDirector = report.EpdDirector };
        return Page();
    }
}
