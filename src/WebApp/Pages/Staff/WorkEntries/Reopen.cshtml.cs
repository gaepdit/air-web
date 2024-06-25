using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.DomainEntities.WorkEntries;
using AirWeb.AppServices.DomainEntities.WorkEntries.Permissions;
using AirWeb.AppServices.Permissions;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;

namespace AirWeb.WebApp.Pages.Staff.WorkEntries;

[Authorize(Policy = nameof(Policies.Manager))]
public class ReopenModel(IWorkEntryService workEntryService, IAuthorizationService authorization) : PageModel
{
    [BindProperty]
    public ChangeEntityStatusDto<int> EntityStatusDto { get; set; } = default!;

    public WorkEntryViewDto ItemView { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null) return RedirectToPage("Index");

        var workEntryView = await workEntryService.FindAsync(id.Value);
        if (workEntryView is null) return NotFound();

        if (!await UserCanReviewAsync(workEntryView)) return Forbid();

        EntityStatusDto = new ChangeEntityStatusDto<int>(id.Value);
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
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "The Work Entry has been reopened.");

        var notificationResult = await workEntryService.ReopenAsync(EntityStatusDto);
        TempData.SetDisplayMessage(
            notificationResult.Success ? DisplayMessage.AlertContext.Success : DisplayMessage.AlertContext.Warning,
            "The WorkEntry has been reopened.", notificationResult.FailureMessage);

        return RedirectToPage("Details", new { id = EntityStatusDto.Id });
    }

    private Task<bool> UserCanReviewAsync(WorkEntryViewDto item) =>
        authorization.Succeeded(User, item, WorkEntryOperation.EditWorkEntry);
}
