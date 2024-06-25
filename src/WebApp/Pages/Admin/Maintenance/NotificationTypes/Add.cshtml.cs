using AirWeb.AppServices.DomainEntities.NotificationTypes;
using AirWeb.AppServices.Permissions;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.NotificationTypes;

[Authorize(Policy = nameof(Policies.SiteMaintainer))]
public class AddModel : PageModel
{
    [BindProperty]
    public NotificationTypeCreateDto Item { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.NotificationType;

    public void OnGet()
    {
        // Method intentionally left empty.
    }

    public async Task<IActionResult> OnPostAsync(
        [FromServices] INotificationTypeService service,
        [FromServices] IValidator<NotificationTypeCreateDto> validator)
    {
        await validator.ApplyValidationAsync(Item, ModelState);
        if (!ModelState.IsValid) return Page();

        var id = await service.CreateAsync(Item.Name);

        HighlightId = id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"“{Item.Name}” successfully added.");
        return RedirectToPage("Index");
    }
}
