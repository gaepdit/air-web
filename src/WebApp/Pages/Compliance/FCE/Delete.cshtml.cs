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
    public StatusCommentDto StatusComment { get; set; } = default!;

    public FceSummaryDto ItemSummary { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");

        var item = await fceService.FindSummaryAsync(Id);
        if (item is null) return NotFound();
        if (!await UserCanDeleteAsync(item)) return Forbid();

        ItemSummary = item;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var item = await fceService.FindSummaryAsync(Id);
        if (item is null || !await UserCanDeleteAsync(item))
            return BadRequest();

        await fceService.DeleteAsync(Id, StatusComment);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "FCE successfully deleted.");
        return RedirectToPage("Details", new { Id });
    }

    private Task<bool> UserCanDeleteAsync(FceSummaryDto item) =>
        authorization.Succeeded(User, item, ComplianceWorkOperation.Delete);
}
