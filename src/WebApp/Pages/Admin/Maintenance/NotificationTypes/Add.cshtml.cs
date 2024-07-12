using AirWeb.AppServices.DomainEntities.NotificationTypes;
using AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.NotificationTypes;

public class AddModel : AddBase
{
    [BindProperty]
    public NotificationTypeCreateDto Item { get; set; } = default!;

    public void OnGet() => ThisOption = MaintenanceOption.NotificationType;

    public async Task<IActionResult> OnPostAsync(
        [FromServices] INotificationTypeService service,
        [FromServices] IValidator<NotificationTypeCreateDto> validator)
    {
        ThisOption = MaintenanceOption.NotificationType;
        return await DoPost(service, validator, Item);
    }
}
