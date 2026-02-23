using AirWeb.AppServices.Sbeap.Cases;
using AirWeb.AppServices.Sbeap.Cases.Dto;
using AirWeb.AppServices.Sbeap.Cases.Permissions;
using AirWeb.WebApp.Models;

namespace AirWeb.WebApp.Pages.SBEAP.Cases;

public class DeleteActionModel(IActionItemService service, ICaseworkService cases, IAuthorizationService authorization)
    : PageModel
{
    [BindProperty]
    public Guid ActionItemId { get; set; }

    public ActionItemViewDto ActionItemView { get; private set; } = null!;
    public CaseworkViewDto CaseView { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(Guid? actionId)
    {
        if (actionId is null) return RedirectToPage("Index");

        var actionItem = await service.FindAsync(actionId.Value);
        if (actionItem is null) return NotFound();

        var caseView = await cases.FindAsync(actionItem.CaseWorkId);
        if (caseView is null) return NotFound();

        if (!await UserCanDeleteActionItemsAsync(caseView)) return Forbid();

        CaseView = caseView;
        ActionItemView = actionItem;
        ActionItemId = actionId.Value;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var originalActionItem = await service.FindAsync(ActionItemId);
        if (originalActionItem is null) return BadRequest();

        var caseView = await cases.FindAsync(originalActionItem.CaseWorkId);
        if (caseView is null || !await UserCanDeleteActionItemsAsync(caseView))
            return BadRequest();

        await service.DeleteAsync(ActionItemId);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "Action Item successfully deleted.");
        return RedirectToPage("Details", new { caseView.Id });
    }

    private async Task<bool> UserCanDeleteActionItemsAsync(CaseworkViewDto item) =>
        (await authorization.AuthorizeAsync(User, item, CaseworkOperation.EditActionItems)).Succeeded;
}
