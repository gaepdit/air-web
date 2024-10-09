using AirWeb.AppServices.Permissions;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace AirWeb.WebApp.Pages.Compliance.TestReport;

[Authorize(Policy = nameof(Policies.Staff))]
public class IndexModel(ISourceTestService testService) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    public SourceTestSummary? Item { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");
        Item = await testService.FindSummaryAsync(Id);
        if (Item is null) return NotFound();
        return Page();
    }
}
