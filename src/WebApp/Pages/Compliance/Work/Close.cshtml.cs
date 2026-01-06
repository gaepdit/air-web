using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.WorkEntryDto.Query;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.WebApp.Models;

namespace AirWeb.WebApp.Pages.Compliance.Work;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class CloseModel(IWorkEntryService entryService) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    public WorkEntrySummaryDto ItemSummary { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");

        var item = await entryService.FindSummaryAsync(Id);
        if (item is null) return NotFound();
        if (item.IsClosed) return BadRequest();
        if (!User.CanClose(item)) return Forbid();

        ItemSummary = item;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (!ModelState.IsValid) return BadRequest();

        var item = await entryService.FindSummaryAsync(Id, token);
        if (item is null || !User.CanClose(item))
            return BadRequest();

        var result = await entryService.CloseAsync(Id, token);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, $"The {item.ItemName} has been closed.");
        if (result.HasWarning) TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, result.WarningMessage);
        return RedirectToPage("Details", new { Id });
    }
}
