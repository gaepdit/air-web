using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Lookups.NotificationTypes;
using AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.NotificationTypes;

[Authorize(Policy = nameof(CompliancePolicies.ComplianceSiteMaintainer))]
public class EditModel(INotificationTypeService service, IValidator<NotificationTypeUpdateDto> validator) : EditBase
{
    [BindProperty]
    public NotificationTypeUpdateDto Item { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        ThisOption = MaintenanceOption.NotificationType;

        if (Id is null) return RedirectToPage("Index");
        var originalItem = await service.FindForUpdateAsync(Id.Value);
        if (originalItem is null) return NotFound();

        OriginalName = originalItem.Name;
        Item = originalItem;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ThisOption = MaintenanceOption.NotificationType;
        return await DoPostAsync(service, validator, Item);
    }
}
