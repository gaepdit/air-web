using AirWeb.AppServices.Compliance.ComplianceMonitoring;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;

namespace AirWeb.WebApp.Pages.Compliance.Work.Edit;

public class EditRedirectModel(IComplianceWorkService service) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");
        var entryType = await service.GetComplianceWorkTypeAsync(Id);
        if (entryType is null) return NotFound();
        return RedirectToPage(entryType switch
        {
            ComplianceWorkType.AnnualComplianceCertification => "ACC",
            ComplianceWorkType.Inspection => "Inspection",
            ComplianceWorkType.Notification => "Notification",
            ComplianceWorkType.PermitRevocation => "PermitRevocation",
            ComplianceWorkType.Report => "Report",
            ComplianceWorkType.RmpInspection => "Inspection",
            ComplianceWorkType.SourceTestReview => "SourceTestReview",
            _ => "Index",
        }, new { Id });
    }
}
