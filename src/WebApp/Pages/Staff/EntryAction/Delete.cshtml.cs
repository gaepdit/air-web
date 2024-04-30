using MyApp.AppServices.EntryActions;
using MyApp.AppServices.EntryActions.Dto;
using MyApp.AppServices.Permissions;
using MyApp.AppServices.Permissions.Helpers;
using MyApp.AppServices.WorkEntries;
using MyApp.AppServices.WorkEntries.Permissions;
using MyApp.AppServices.WorkEntries.QueryDto;
using MyApp.WebApp.Models;
using MyApp.WebApp.Platform.PageModelHelpers;

namespace MyApp.WebApp.Pages.Staff.EntryAction;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class DeleteActionModel(
    IEntryActionService actionService,
    IWorkEntryService workEntryService,
    IAuthorizationService authorization) : PageModel
{
    [BindProperty]
    public Guid EntryActionItemId { get; set; }

    [TempData]
    public Guid HighlightId { get; set; }

    public EntryActionViewDto EntryActionViewDto { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? actionId)
    {
        if (actionId is null) return RedirectToPage("Index");

        var actionItem = await actionService.FindAsync(actionId.Value);
        if (actionItem is null) return NotFound();

        var workEntryView = await workEntryService.FindAsync(actionItem.WorkEntryId);
        if (workEntryView is null) return NotFound();

        if (!await UserCanManageDeletionsAsync(workEntryView)) return Forbid();

        if (actionItem.IsDeleted)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "WorkEntry Action cannot be deleted because it is already deleted.");
            return RedirectToPage("../WorkEntries/Details", routeValues: new { workEntryView.Id });
        }

        EntryActionViewDto = actionItem;
        EntryActionItemId = actionId.Value;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var originalActionItem = await actionService.FindAsync(EntryActionItemId);
        if (originalActionItem is null || originalActionItem.IsDeleted) return BadRequest();

        var workEntryView = await workEntryService.FindAsync(originalActionItem.WorkEntryId);
        if (workEntryView is null || !await UserCanManageDeletionsAsync(workEntryView))
            return BadRequest();

        await actionService.DeleteAsync(EntryActionItemId);
        HighlightId = EntryActionItemId;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "WorkEntry Action successfully deleted.");
        return RedirectToPage("../WorkEntries/Details", pageHandler: null, routeValues: new { workEntryView.Id },
            fragment: HighlightId.ToString());
    }

    private Task<bool> UserCanManageDeletionsAsync(WorkEntryViewDto item) =>
        authorization.Succeeded(User, item, WorkEntryOperation.ManageDeletions);
}
