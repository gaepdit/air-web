using AirWeb.AppServices.CommonDto;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.WorkEntries;
using AirWeb.AppServices.WorkEntries.Permissions;
using AirWeb.AppServices.WorkEntries.QueryDto;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;

namespace AirWeb.WebApp.Pages.Staff.WorkEntries;

[Authorize(Policy = nameof(Policies.Manager))]
public class CloseModel(IWorkEntryService workEntryService, IAuthorizationService authorization) : PageModel
{
    [BindProperty]
    public ChangeEntityStatusDto<Guid> EntityStatusDto { get; set; } = default!;

    public WorkEntryViewDto ItemView { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");

        var workEntryView = await workEntryService.FindAsync(id.Value);
        if (workEntryView is null) return NotFound();

        if (!await UserCanReviewAsync(workEntryView)) return Forbid();

        EntityStatusDto = new ChangeEntityStatusDto<Guid>(id.Value);
        ItemView = workEntryView;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var workEntryView = await workEntryService.FindAsync(EntityStatusDto.Id);
        if (workEntryView is null || !await UserCanReviewAsync(workEntryView))
            return BadRequest();

        await workEntryService.CloseAsync(EntityStatusDto);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "The Work Entry has been closed.");

        return RedirectToPage("Details", new { id = EntityStatusDto.Id });
    }

    private Task<bool> UserCanReviewAsync(WorkEntryViewDto item) =>
        authorization.Succeeded(User, item, WorkEntryOperation.EditWorkEntry);
}
