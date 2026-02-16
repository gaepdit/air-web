using AirWeb.AppServices.Compliance.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Notifications;
using AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.NotificationTypes;

[Authorize(Policy = nameof(CompliancePolicies.ComplianceSiteMaintainer))]
public class AddModel : AddBase
{
    [BindProperty]
    public NotificationTypeCreateDto Item { get; set; } = null!;

    public void OnGet()
    {
        ThisOption = MaintenanceOption.NotificationType;
        Item = new NotificationTypeCreateDto();
    }

    public async Task<IActionResult> OnPostAsync(
        [FromServices] INotificationTypeService service,
        [FromServices] IValidator<NotificationTypeCreateDto> validator)
    {
        ThisOption = MaintenanceOption.NotificationType;
        return await DoPostAsync(service, validator, Item);
    }
}
