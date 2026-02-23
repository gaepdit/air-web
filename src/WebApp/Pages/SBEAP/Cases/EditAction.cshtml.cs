using AirWeb.AppServices.Sbeap.ActionItemTypes;
using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.AppServices.Sbeap.Cases;
using AirWeb.AppServices.Sbeap.Cases.Dto;
using AirWeb.AppServices.Sbeap.Cases.Permissions;
using AirWeb.WebApp.Models;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.SBEAP.Cases;

[Authorize(Policy = nameof(SbeapPolicies.SbeapStaff))]
public class EditActionModel(
    IActionItemService service,
    ICaseworkService cases,
    IActionItemTypeService actionItemTypes,
    IAuthorizationService authorization)
    : PageModel
{
    // Properties

    [FromRoute]
    public Guid ActionId { get; set; }

    [BindProperty]
    public ActionItemUpdateDto ActionItemUpdate { get; set; } = null!;

    [TempData]
    public Guid HighlightId { get; set; }

    public CaseworkSearchResultDto CaseView { get; private set; } = null!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    // Select lists
    public SelectList ActionItemTypeSelectList { get; private set; } = null!;

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? actionId)
    {
        if (actionId is null) return RedirectToPage("../Index");

        var actionItem = await service.FindForUpdateAsync(actionId.Value);
        if (actionItem is null) return NotFound();

        var caseView = await cases.FindBasicInfoAsync(actionItem.CaseWorkId);
        if (caseView is null) return NotFound();

        await SetPermissionsAsync(actionItem);

        if (UserCan[CaseworkOperation.EditActionItems])
        {
            CaseView = caseView;
            ActionId = actionId.Value;
            ActionItemUpdate = actionItem;
            await PopulateSelectListsAsync();
            return Page();
        }

        if (!UserCan[CaseworkOperation.ManageDeletions] || ActionItemUpdate.IsDeleted)
            return NotFound();

        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Info, "Cannot edit a deleted case.");
        return RedirectToPage("Details", new { id = actionItem.CaseWorkId });
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var originalActionItem = await service.FindForUpdateAsync(ActionId);
        if (originalActionItem is null) return BadRequest();

        await SetPermissionsAsync(originalActionItem);
        if (!UserCan[CaseworkOperation.EditActionItems]) return BadRequest();

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        await service.UpdateAsync(ActionId, ActionItemUpdate);

        HighlightId = ActionId;
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "Action Item successfully updated.");
        return RedirectToPage("Details", new { id = originalActionItem.CaseWorkId });
    }

    private async Task PopulateSelectListsAsync() =>
        ActionItemTypeSelectList = (await actionItemTypes.GetAsListItemsAsync()).ToSelectList();

    private async Task SetPermissionsAsync(ActionItemUpdateDto item)
    {
        foreach (var operation in CaseworkOperation.AllOperations)
            UserCan[operation] = (await authorization.AuthorizeAsync(User, item, operation)).Succeeded;
    }
}
