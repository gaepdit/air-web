using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.Notifications;
using AirWeb.AppServices.NamedEntities.NotificationTypes;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.Staff;
using AirWeb.WebApp.Pages.Compliance.Work.WorkEntryBase;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Compliance.Work.Notification;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class EditModel(
    IWorkEntryService entryService,
    INotificationTypeService notificationTypeService,
    IStaffService staffService,
    IAuthorizationService authorization,
    IValidator<NotificationUpdateDto> validator)
    : EditBase(entryService, staffService)
{
    private readonly IWorkEntryService _entryService = entryService;

    [BindProperty]
    public NotificationUpdateDto Item { get; set; } = default!;

    public SelectList NotificationTypeSelectList { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("../Index");

        var item = (NotificationUpdateDto?)await _entryService.FindForUpdateAsync(Id);
        if (item is null) return NotFound();
        if (!await UserCanEditAsync(item)) return Forbid();
        Item = item;

        return await DoGetAsync();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var original = (NotificationUpdateDto?)await _entryService.FindForUpdateAsync(Id, token);
        if (original is null || !await UserCanEditAsync(original)) return BadRequest();

        return await DoPostAsync(Item, validator, token);
    }

    protected override async Task PopulateSelectListsAsync()
    {
        await base.PopulateSelectListsAsync();
        NotificationTypeSelectList = (await notificationTypeService.GetAsListItemsAsync()).ToSelectList();
    }

    private Task<bool> UserCanEditAsync(NotificationUpdateDto item) =>
        authorization.Succeeded(User, item, new NotificationUpdateRequirement());
}
