using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;

namespace AirWeb.WebApp.Pages.Compliance.FCE;

[Authorize(Policy = nameof(Policies.ComplianceManager))]
public class DeleteModel(IFceService fceService, IAuthorizationService authorization) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty]
    public StatusCommentDto StatusComment { get; set; } = null!;

    public FceSummaryDto ItemSummary { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");

        var item = await fceService.FindSummaryAsync(Id);
        if (item is null) return NotFound();
        if (!await UserCanDeleteAsync(item)) return Forbid();

        ItemSummary = item;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (!ModelState.IsValid) return BadRequest();

        var item = await fceService.FindSummaryAsync(Id, token);
        if (item is null || !await UserCanDeleteAsync(item))
            return BadRequest();

        var notificationResult = await fceService.DeleteAsync(Id, StatusComment, token);
        TempData.SetDisplayMessage(
            notificationResult.Success ? DisplayMessage.AlertContext.Success : DisplayMessage.AlertContext.Warning,
            "FCE successfully deleted.", notificationResult.FailureMessage);

        return RedirectToPage("Details", new { Id });
    }

    private Task<bool> UserCanDeleteAsync(FceSummaryDto item) =>
        authorization.Succeeded(User, item, ComplianceWorkOperation.Delete);
}
