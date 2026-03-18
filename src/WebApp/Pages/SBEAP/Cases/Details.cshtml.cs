using AirWeb.AppServices.Sbeap.ActionItemTypes;
using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.AppServices.Sbeap.Cases;
using AirWeb.AppServices.Sbeap.Cases.Dto;
using AirWeb.AppServices.Sbeap.Cases.Permissions;
using AirWeb.WebApp.Models;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.SBEAP.Cases;

[Authorize(Policy = nameof(SbeapPolicies.SbeapStaff))]
public class DetailsModel(
    ICaseworkService cases,
    IActionItemService actionItems,
    IActionItemTypeService actionItemTypes,
    IAuthorizationService authorization)
    : PageModel
{
    public CaseworkViewDto Item { get; private set; } = null!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    [BindProperty]
    public ActionItemCreateDto NewActionItem { get; set; } = null!;

    [TempData]
    public Guid HighlightId { get; set; }

    public SelectList ActionItemTypeSelectList { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");
        var item = await cases.FindAsync(id.Value);
        if (item is null) return NotFound();

        await SetPermissionsAsync(item);
        if (item.IsDeleted && !UserCan[CaseworkOperation.ManageDeletions]) return NotFound();

        Item = item;
        NewActionItem = new ActionItemCreateDto(id.Value);
        await PopulateSelectListsAsync();
        return Page();
    }

    /// <summary>
    /// Post is used to add a new Action Item for this Case
    /// </summary>
    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");
        if (NewActionItem.CaseworkId != id) return BadRequest();

        var item = await cases.FindAsync(id.Value);
        if (item is null || item.IsDeleted) return BadRequest();

        await SetPermissionsAsync(item);
        if (!UserCan[CaseworkOperation.EditActionItems]) return BadRequest();

        if (!ModelState.IsValid)
        {
            Item = item;
            await PopulateSelectListsAsync();
            return Page();
        }

        HighlightId = await actionItems.CreateAsync(NewActionItem);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "New Action successfully added.");
        return RedirectToPage("Details", new { id });
    }

    private async Task PopulateSelectListsAsync() =>
        ActionItemTypeSelectList = (await actionItemTypes.GetAsListItemsAsync()).ToSelectList();

    private async Task SetPermissionsAsync(CaseworkViewDto item)
    {
        foreach (var operation in CaseworkOperation.AllOperations)
            UserCan[operation] = (await authorization.AuthorizeAsync(User, item, operation)).Succeeded;
    }
}
