using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Notifications;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using GaEpd.AppLibrary.ListItems;
using IaipDataService.Facilities;
using Microsoft.Identity.Web;

namespace AirWeb.WebApp.Pages.Compliance.Work.Add;

public class NotificationAddModel(
    IFacilityService facilityService,
    IComplianceWorkService complianceService,
    INotificationTypeService notificationTypeService,
    IStaffService staffService,
    IValidator<NotificationCreateDto> validator)
    : AddBase(facilityService, complianceService, null, staffService)
{
    [BindProperty]
    public NotificationCreateDto Item { get; set; } = null!;

    public SelectList NotificationTypeSelectList { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(CancellationToken token = default)
    {
        WorkType = ComplianceWorkType.Notification;
        Item = new NotificationCreateDto { ResponsibleStaffId = User.GetNameIdentifierId() };
        return await DoGetAsync(token);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        WorkType = ComplianceWorkType.Notification;
        return await DoPostAsync(Item, validator, token);
    }

    protected override async Task PopulateSelectListsAsync(CancellationToken token)
    {
        await base.PopulateSelectListsAsync(token);
        NotificationTypeSelectList = (await notificationTypeService.GetAsListItemsAsync(token: token)).ToSelectList();
    }
}
