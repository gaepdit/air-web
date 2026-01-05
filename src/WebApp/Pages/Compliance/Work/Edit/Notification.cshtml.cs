using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.Notifications;
using AirWeb.AppServices.Lookups.NotificationTypes;
using AirWeb.AppServices.Staff;
using AutoMapper;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Compliance.Work.Edit;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class NotificationEditModel(
    IWorkEntryService entryService,
    INotificationTypeService notificationTypeService,
    IStaffService staffService,
    IMapper mapper,
    IValidator<NotificationUpdateDto> validator)
    : EditBase(entryService, staffService, mapper)
{
    [BindProperty]
    public NotificationUpdateDto Item { get; set; } = null!;

    public SelectList NotificationTypeSelectList { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        var result = await DoGetAsync(token);
        if (result is not PageResult) return result;
        Item = Mapper.Map<NotificationUpdateDto>(ItemView);
        return result;
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token) =>
        await DoPostAsync(Item, validator, token);

    protected override async Task PopulateSelectListsAsync()
    {
        await base.PopulateSelectListsAsync();
        NotificationTypeSelectList = (await notificationTypeService.GetAsListItemsAsync()).ToSelectList();
    }
}
