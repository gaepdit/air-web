using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.Domain.ComplianceEntities.WorkEntries;

namespace AirWeb.WebApp.Pages.Compliance.Work;

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
            WorkEntryType.AnnualComplianceCertification => "Acc/Edit",
            WorkEntryType.Inspection => "Inspection/Edit",
            WorkEntryType.Notification => "Notification/Edit",
            WorkEntryType.PermitRevocation => "PermitRevocation/Edit",
            WorkEntryType.Report => "Report/Edit",
            WorkEntryType.RmpInspection => "Inspection/Edit",
            WorkEntryType.SourceTestReview => "Str/Edit",
            _ => "Index",
        }, new { Id });
    }
}
