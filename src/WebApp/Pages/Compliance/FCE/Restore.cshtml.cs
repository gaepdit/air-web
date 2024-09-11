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

    public FceViewDto ItemView { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");

        var itemView = await fceService.FindAsync(Id);
        if (itemView is null) return NotFound();
        if (!await UserCanRestoreAsync(itemView)) return Forbid();

        ItemView = itemView;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var itemView = await fceService.FindAsync(Id);
        if (itemView is null || !itemView.IsDeleted || !await UserCanRestoreAsync(itemView))
            return BadRequest();

        await fceService.RestoreAsync(Id);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "FCE successfully restored.");
        return RedirectToPage("Details", new { Id });
    }

    private Task<bool> UserCanRestoreAsync(FceViewDto item) =>
        authorization.Succeeded(User, item, ComplianceWorkOperation.Restore);
}
