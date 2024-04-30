using MyApp.AppServices.EntryActions;
using MyApp.AppServices.EntryActions.Dto;
using MyApp.AppServices.Permissions;
using MyApp.AppServices.Permissions.Helpers;
using MyApp.AppServices.WorkEntries;
using MyApp.AppServices.WorkEntries.Permissions;
using MyApp.AppServices.WorkEntries.QueryDto;
using MyApp.WebApp.Models;
using MyApp.WebApp.Platform.PageModelHelpers;

namespace MyApp.WebApp.Pages.Staff.WorkEntries;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class DetailsModel(
    IWorkEntryService workEntryService,
    IEntryActionService entryActionService,
    IAuthorizationService authorization) : PageModel
{
    public WorkEntryViewDto ItemView { get; private set; } = default!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; private set; } = new();
    public EntryActionCreateDto NewAction { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public bool ViewableActions => ItemView.EntryActions.Exists(action =>
        !action.IsDeleted || UserCan[WorkEntryOperation.ViewDeletedActions]);

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");

        var workEntryView = await workEntryService.FindAsync(id.Value, true);
        if (workEntryView is null) return NotFound();

        await SetPermissionsAsync(workEntryView);
        if (workEntryView.IsDeleted && !UserCan[WorkEntryOperation.ManageDeletions]) return NotFound();

        ItemView = workEntryView;
        NewAction = new EntryActionCreateDto(workEntryView.Id);
        return Page();
    }

    /// PostNewAction is used to add a new Action for this WorkEntry.
    public async Task<IActionResult> OnPostNewActionAsync(Guid? id, EntryActionCreateDto newAction,
        CancellationToken token)
    {
        if (id is null || newAction.WorkEntryId != id) return BadRequest();

        var workEntryView = await workEntryService.FindAsync(id.Value, includeDeletedActions: true, token);
        if (workEntryView is null || workEntryView.IsDeleted) return BadRequest();

        await SetPermissionsAsync(workEntryView);
        if (!UserCan[WorkEntryOperation.EditWorkEntry]) return BadRequest();

        if (!ModelState.IsValid)
        {
            ItemView = workEntryView;
            return Page();
        }

        HighlightId = await entryActionService.CreateAsync(newAction, token);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "New Entry Action successfully added.");
        return RedirectToPage("Details", pageHandler: null, routeValues: new { id }, fragment: HighlightId.ToString());
    }

    private async Task SetPermissionsAsync(WorkEntryViewDto item)
    {
        foreach (var operation in WorkEntryOperation.AllOperations)
            UserCan[operation] = await authorization.Succeeded(User, item, operation);
    }
}
