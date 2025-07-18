using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.WebApp.Models;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class DeleteModel(ICaseFileService service) : PageModel
{
    [FromRoute]
    public int Id { get; set; } // Case File ID

    [BindProperty]
    public CommentDto Comment { get; set; } = null!;

    public CaseFileSummaryDto ItemSummary { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        if (Id == 0) return RedirectToPage("Index");

        var item = await service.FindSummaryAsync(Id, token);
        if (item is null) return NotFound();
        if (!User.CanDeleteCaseFile(item)) return Forbid();

        ItemSummary = item;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (!ModelState.IsValid) return BadRequest();

        var item = await service.FindSummaryAsync(Id, token);
        if (item is null || !User.CanDeleteCaseFile(item))
            return BadRequest();

        var result = await service.DeleteAsync(Id, Comment, token);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "Enforcement Case successfully deleted.");
        if (result.HasWarning) TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, result.WarningMessage);
        return RedirectToPage("Details", new { Id });
    }
}
