using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Compliance;
using AirWeb.AppServices.Compliance.Fces;
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
    public StatusCommentDto Entity { get; set; } = default!;

    public FceViewDto ItemView { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");

        var itemView = await fceService.FindAsync(Id);
        if (itemView is null) return NotFound();
        if (!await UserCanDeleteAsync(itemView)) return Forbid();

        ItemView = itemView;
        Entity = new StatusCommentDto();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var itemView = await fceService.FindAsync(Id);
        if (itemView is null || itemView.IsDeleted || !await UserCanDeleteAsync(itemView))
            return BadRequest();

        await fceService.DeleteAsync(Id, Entity);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "FCE successfully deleted.");
        return RedirectToPage("Details", new { Id });
    }

    private Task<bool> UserCanDeleteAsync(FceViewDto item) =>
        authorization.Succeeded(User, item, ComplianceWorkOperation.Delete);
}
