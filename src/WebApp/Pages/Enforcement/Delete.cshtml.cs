using AirWeb.AppServices.Compliance.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.Enforcement;
using AirWeb.AppServices.Compliance.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Compliance.Enforcement.Permissions;
using AirWeb.AppServices.Core.CommonDtos;
using AirWeb.WebApp.Models;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(CompliancePolicies.ComplianceStaff))]
public class DeleteModel(ICaseFileService service) : PageModel
{
    [FromRoute]
    public int Id { get; set; } // Case File ID

    [BindProperty]
    public CommentDto Comment { get; set; } = null!;

    public CaseFileViewDto ItemSummary { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        if (Id == 0) return RedirectToPage("Index");

        var item = await service.FindDetailedAsync(Id, token);
        if (item is null) return NotFound();
        if (!User.CanDeleteCaseFile(item)) return Forbid();

        ItemSummary = item;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (!ModelState.IsValid) return BadRequest();

        var item = await service.FindDetailedAsync(Id, token);
        if (item is null || !User.CanDeleteCaseFile(item))
            return BadRequest();

        var result = await service.DeleteAsync(Id, Comment, token);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "Enforcement Case successfully deleted.");
        if (result.HasWarning) TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, result.WarningMessage);
        return RedirectToPage("Details", new { Id });
    }
}
