using AirWeb.AppServices.Compliance.ComplianceWork;
using AirWeb.AppServices.Compliance.ComplianceWork.Notifications;
using AirWeb.AppServices.Lookups.NotificationTypes;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Compliance.Work.Add;

public class NotificationAddModel(
    IWorkEntryService entryService,
    IFacilityService facilityService,
    INotificationTypeService notificationTypeService,
    IStaffService staffService,
    IValidator<NotificationCreateDto> validator)
    : AddBase(facilityService, staffService)
{
    private readonly IStaffService _staffService = staffService;

    [BindProperty]
    public NotificationCreateDto Item { get; set; } = null!;

    public SelectList NotificationTypeSelectList { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        EntryType = WorkEntryType.Notification;

        Item = new NotificationCreateDto
        {
            FacilityId = FacilityId,
            ResponsibleStaffId = (await _staffService.GetCurrentUserAsync()).Id,
        };

        return await DoGetAsync();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        EntryType = WorkEntryType.Notification;
        return await DoPostAsync(Item, entryService, validator, token);
    }

    protected override async Task PopulateSelectListsAsync()
    {
        await base.PopulateSelectListsAsync();
        NotificationTypeSelectList = (await notificationTypeService.GetAsListItemsAsync()).ToSelectList();
    }
}
