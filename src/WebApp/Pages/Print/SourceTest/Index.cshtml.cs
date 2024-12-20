using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.WebApp.Platform.PrintoutModels;
using AirWeb.WebApp.Platform.Settings;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace AirWeb.WebApp.Pages.Print.SourceTest;

public class IndexModel : PageModel
{
    public BaseSourceTestReport? Report { get; private set; }
    public MemoHeader MemoHeader { get; private set; }
    public OrganizationInfo OrganizationInfo { get; private set; } = null!;
    public bool ShowConfidentialWarning { get; private set; }

    public async Task<ActionResult> OnGetAsync(
        [FromServices] ISourceTestService sourceTestService,
        [FromServices] IAuthorizationService authorizationService,
        [FromRoute] int referenceNumber,
        [FromQuery] bool includeConfidentialInfo = false)
    {
        if (includeConfidentialInfo)
        {
            var activeUser = await authorizationService.Succeeded(User, Policies.ActiveUser);
            if (!activeUser) return Challenge();
        }

        Report = await sourceTestService.FindAsync(referenceNumber);
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
