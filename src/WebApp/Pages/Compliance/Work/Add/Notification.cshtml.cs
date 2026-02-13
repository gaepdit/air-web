using AirWeb.AppServices.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.Notifications;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Compliance.Work.Add;

public class NotificationAddModel(
    IComplianceWorkService service,
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
        WorkType = ComplianceWorkType.Notification;

        Item = new NotificationCreateDto
        {
            FacilityId = FacilityId,
            ResponsibleStaffId = (await _staffService.GetCurrentUserAsync()).Id,
        };

        return await DoGetAsync();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        WorkType = ComplianceWorkType.Notification;
        return await DoPostAsync(Item, service, validator, token);
    }

    protected override async Task PopulateSelectListsAsync()
    {
        await base.PopulateSelectListsAsync();
        NotificationTypeSelectList = (await notificationTypeService.GetAsListItemsAsync()).ToSelectList();
    }
}
