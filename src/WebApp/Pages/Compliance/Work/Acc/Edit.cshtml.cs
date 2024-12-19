using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.Accs;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.Staff;
using AirWeb.WebApp.Pages.Compliance.Work.WorkEntryBase;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Compliance.Work.Acc;

public class EditModel(
    IWorkEntryService entryService,
    IStaffService staffService,
    IAuthorizationService authorization,
    IValidator<AccUpdateDto> validator)
    : EditBase(entryService, staffService)
{
    private readonly IWorkEntryService _entryService = entryService;

    [BindProperty]
    public AccUpdateDto Item { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("../Index");

        var item = (AccUpdateDto?)await _entryService.FindForUpdateAsync(Id);
        if (item is null) return NotFound();
        if (!await authorization.UserCanEditAsync(User, item)) return Forbid();
        Item = item;

        return await DoGetAsync();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var original = (AccUpdateDto?)await _entryService.FindForUpdateAsync(Id, token);
        if (original is null || !await authorization.UserCanEditAsync(User, original)) return BadRequest();

        return await DoPostAsync(Item, validator, token);
    }
}
