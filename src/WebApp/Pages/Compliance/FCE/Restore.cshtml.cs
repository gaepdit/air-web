using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.WebApp.Models;

namespace AirWeb.WebApp.Pages.Compliance.FCE;

[Authorize(Policy = nameof(Policies.ComplianceManager))]
public class RestoreModel(IFceService fceService) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    public FceSummaryDto ItemSummary { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");

        var item = await fceService.FindSummaryAsync(Id);
        if (item is null) return NotFound();
        if (!User.CanRestore(item)) return Forbid();

        ItemSummary = item;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (!ModelState.IsValid) return BadRequest();

        var item = await fceService.FindSummaryAsync(Id, token);
        if (item is null || !item.IsDeleted || !User.CanRestore(item))
            return BadRequest();

        var result = await fceService.RestoreAsync(Id, token);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "FCE successfully restored.");
        if (result.HasWarning) TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, result.WarningMessage);

        return RedirectToPage("Details", new { Id });
    }
}
