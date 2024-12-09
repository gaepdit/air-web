using AirWeb.Domain.Identity;
using AirWeb.WebApp.Platform.ReportsModels;
using AirWeb.WebApp.Platform.Settings;
using IaipDataService.Facilities;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace AirWeb.WebApp.Pages.Report.StackTest;

public class IndexModel : PageModel
{
    public BaseSourceTestReport? Report { get; private set; }
    public MemoHeader MemoHeader { get; private set; }
    public OrganizationInfo OrganizationInfo { get; private set; } = default!;
    public bool ShowConfidentialWarning { get; private set; }

    public async Task<ActionResult> OnGetAsync(
        [FromServices] ISourceTestService sourceTestService,
        [FromRoute] int referenceNumber,
        [FromQuery] bool includeConfidentialInfo = false)
    {
        if (includeConfidentialInfo)
        {
            if (User.Identity is not { IsAuthenticated: true }) return Challenge();
            if (User.Identity.Name is null || !User.Identity.Name.IsValidEmailDomain()) return Forbid();
        }

        var sourceTestTask = sourceTestService.FindAsync(referenceNumber);
        Report = await sourceTestTask;
        if (Report?.Facility == null) return NotFound();

        Report = includeConfidentialInfo ? Report : Report.RedactedStackTestReport();
        MemoHeader = new MemoHeader
        {
            To = Report.ComplianceManager.DisplayName,
            From = Report.ReviewedByStaff.DisplayName,
            Through = Report.TestingUnitManager.DisplayName,
            Subject = Report.ReportTypeSubject.ToUpperInvariant(),
        };
        ShowConfidentialWarning = includeConfidentialInfo && Report.ConfidentialParameters.Count != 0;
        OrganizationInfo = AppSettings.OrganizationInfo with { NameOfDirector = Report.EpdDirector };
        return Page();
    }
}
