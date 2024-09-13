using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.Reports;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.Staff;
using AirWeb.WebApp.Pages.Compliance.Work.WorkEntryBase;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Compliance.Work.Report;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class EditModel(
    IWorkEntryService entryService,
    IStaffService staffService,
    IAuthorizationService authorization,
    IValidator<ReportUpdateDto> validator)
    : EditBase(entryService, staffService)
{
    private readonly IWorkEntryService _entryService = entryService;

    [BindProperty]
    public ReportUpdateDto Item { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("../Index");

        var item = (ReportUpdateDto?)await _entryService.FindForUpdateAsync(Id);
        if (item is null) return NotFound();
        if (!await UserCanEditAsync(item)) return Forbid();
        Item = item;

        return await DoGetAsync();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var original = (ReportUpdateDto?)await _entryService.FindForUpdateAsync(Id, token);
        if (original is null || !await UserCanEditAsync(original)) return BadRequest();

        return await DoPostAsync(Item, validator, token);
    }

    private Task<bool> UserCanEditAsync(ReportUpdateDto item) =>
        authorization.Succeeded(User, item, new ReportUpdateRequirement());
}
