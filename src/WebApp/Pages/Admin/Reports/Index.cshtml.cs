using AirWeb.AppServices.Core.AuthorizationServices;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Admin.Reports;

[Authorize(Policy = nameof(Policies.Staff))]
public class ReportsIndexModel(IFacilityService facilityService) : PageModel
{
    [TempData]
    public string DownloadHandler { get; set; } = string.Empty;

    public IActionResult OnGetFceInspectionStatus(CancellationToken token = default)
    {
        DownloadHandler = "FceInspectionFile";
        return RedirectToPage("Index");
    }

    public async Task<IActionResult> OnGetFceInspectionFileAsync(CancellationToken token = default)
    {
        var report = await facilityService.GetFceInspectionStatusReportAsync(token);
        return this.ExcelFile(report, "FCE Inspection Status", "FCE_Inspection_Status");
    }
}
