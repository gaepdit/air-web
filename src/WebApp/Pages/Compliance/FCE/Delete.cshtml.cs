using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.WebApp.Models;

namespace AirWeb.WebApp.Pages.Compliance.FCE;

[Authorize(Policy = nameof(CompliancePolicies.ComplianceManager))]
public class DeleteModel(IFceService fceService) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty]
    public CommentDto Comment { get; set; } = null!;

    public FceSummaryDto ItemSummary { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");

        var item = await fceService.FindSummaryAsync(Id);
        if (item is null) return NotFound();
        if (!User.CanDelete(item)) return Forbid();

        ItemSummary = item;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (!ModelState.IsValid) return BadRequest();

        var item = await fceService.FindSummaryAsync(Id, token);
        if (item is null || !User.CanDelete(item))
            return BadRequest();

        var result = await fceService.DeleteAsync(Id, Comment, token);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "FCE successfully deleted.");
        if (result.HasWarning) TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, result.WarningMessage);
        return RedirectToPage("Details", new { Id });
    }
}
