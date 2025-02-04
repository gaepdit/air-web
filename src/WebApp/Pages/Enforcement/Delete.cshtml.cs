using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.CaseFiles;
using AirWeb.AppServices.Permissions;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class DeleteModel(IEnforcementService service) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty]
    public StatusCommentDto StatusComment { get; set; } = null!;

    public CaseFileSummaryDto ItemSummary { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        if (Id == 0) return RedirectToPage("Index");

        var item = await service.FindCaseFileSummaryAsync(Id, token);
        if (item is null) return NotFound();
        if (!User.CanDelete(item)) return Forbid();

        ItemSummary = item;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (!ModelState.IsValid) return BadRequest();

        var item = await service.FindCaseFileSummaryAsync(Id, token);
        if (item is null || !User.CanDelete(item))
            return BadRequest();

        var notificationResult = await service.DeleteCaseFileAsync(Id, StatusComment, token);
        TempData.SetDisplayMessage(
            notificationResult.Success ? DisplayMessage.AlertContext.Success : DisplayMessage.AlertContext.Warning,
            $"Enforcement Case successfully deleted.", notificationResult.FailureMessage);

        return RedirectToPage("Details", new { Id });
    }
}
