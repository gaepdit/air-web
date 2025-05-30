using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.AppServices.Permissions;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class ReopenModel(ICaseFileService service) : PageModel
{
    [FromRoute]
    public int Id { get; set; } // Case File ID

    public CaseFileSummaryDto ItemSummary { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        if (Id == 0) return RedirectToPage("Index");

        var item = await service.FindSummaryAsync(Id, token);
        if (item is null) return NotFound();
        if (!item.IsClosed) return BadRequest();
        if (!User.CanReopenCaseFile(item)) return Forbid();

        ItemSummary = item;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (!ModelState.IsValid) return BadRequest();

        var item = await service.FindSummaryAsync(Id, token);
        if (item is null || !User.CanReopenCaseFile(item))
            return BadRequest();

        var notificationResult = await service.ReopenAsync(Id, token);
        TempData.SetDisplayMessage(
            notificationResult.Success ? DisplayMessage.AlertContext.Success : DisplayMessage.AlertContext.Warning,
            $"The Enforcement Case has been reopened.", notificationResult.FailureMessage);

        return RedirectToPage("Details", new { Id });
    }
}
