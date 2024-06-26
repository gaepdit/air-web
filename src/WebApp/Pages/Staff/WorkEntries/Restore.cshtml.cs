using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.DomainEntities.WorkEntries;
using AirWeb.AppServices.DomainEntities.WorkEntries.Permissions;
using AirWeb.AppServices.Permissions;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;

namespace AirWeb.WebApp.Pages.Staff.WorkEntries;

[Authorize(Policy = nameof(Policies.Manager))]
public class RestoreModel(IWorkEntryService workEntryService, IAuthorizationService authorization) : PageModel
{
    [BindProperty]
    public ChangeEntityStatusDto<int> EntityStatusDto { get; set; } = default!;

    public WorkEntryViewDto ItemView { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null) return RedirectToPage("Index");

        var workEntryView = await workEntryService.FindAsync(id.Value);
        if (workEntryView is null) return NotFound();

        if (!await UserCanManageDeletionsAsync(workEntryView)) return Forbid();

        if (ItemView.IsDeleted)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "WorkEntry cannot be restored because it is not deleted.");
            return RedirectToPage("Details", routeValues: new { id });
        }

        EntityStatusDto = new ChangeEntityStatusDto<int>(id.Value);
        ItemView = workEntryView;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var workEntryView = await workEntryService.FindAsync(EntityStatusDto.Id);
        if (workEntryView is null || workEntryView.IsDeleted || !await UserCanManageDeletionsAsync(workEntryView))
            return BadRequest();

        await workEntryService.RestoreAsync(EntityStatusDto);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Work Entry successfully restored.");

        return RedirectToPage("Details", new { id = EntityStatusDto.Id });
    }

    private Task<bool> UserCanManageDeletionsAsync(WorkEntryViewDto item) =>
        authorization.Succeeded(User, item, WorkEntryOperation.ManageDeletions);
}
