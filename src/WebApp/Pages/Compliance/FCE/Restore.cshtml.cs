using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;

namespace AirWeb.WebApp.Pages.Compliance.FCE;

[Authorize(Policy = nameof(Policies.ComplianceManager))]
public class RestoreModel(IFceService fceService, IAuthorizationService authorization) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    public FceSummaryDto ItemSummary { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");

        var item = await fceService.FindSummaryAsync(Id);
        if (item is null) return NotFound();
        if (!await UserCanRestoreAsync(item)) return Forbid();

        ItemSummary = item;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (!ModelState.IsValid) return BadRequest();

        var item = await fceService.FindSummaryAsync(Id, token);
        if (item is null || !item.IsDeleted || !await UserCanRestoreAsync(item))
            return BadRequest();

        var notificationResult = await fceService.RestoreAsync(Id, token);
        TempData.SetDisplayMessage(
            notificationResult.Success ? DisplayMessage.AlertContext.Success : DisplayMessage.AlertContext.Warning,
            "FCE successfully restored.", notificationResult.FailureMessage);

        return RedirectToPage("Details", new { Id });
    }

    private Task<bool> UserCanRestoreAsync(FceSummaryDto item) =>
        authorization.Succeeded(User, item, ComplianceWorkOperation.Restore);
}
