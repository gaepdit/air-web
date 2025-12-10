using AirWeb.AppServices.Compliance.ComplianceWork;
using AirWeb.Domain.ComplianceEntities.ComplianceWork;

namespace AirWeb.WebApp.Pages.Compliance.Work.Edit;

public class EditRedirectModel(IWorkEntryService entryService) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");
        var entryType = await entryService.GetWorkEntryTypeAsync(Id);
        if (entryType is null) return NotFound();
        return RedirectToPage(entryType switch
        {
            WorkEntryType.AnnualComplianceCertification => "ACC",
            WorkEntryType.Inspection => "Inspection",
            WorkEntryType.Notification => "Notification",
            WorkEntryType.PermitRevocation => "PermitRevocation",
            WorkEntryType.Report => "Report",
            WorkEntryType.RmpInspection => "Inspection",
            WorkEntryType.SourceTestReview => "SourceTestReview",
            _ => "Index",
        }, new { Id });
    }
}
