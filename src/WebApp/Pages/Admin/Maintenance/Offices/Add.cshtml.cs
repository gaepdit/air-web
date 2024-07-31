using AirWeb.AppServices.NamedEntities.Offices;
using AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.Offices;

public class AddModel : AddBase
{
    [BindProperty]
    public OfficeCreateDto Item { get; set; } = default!;

    public void OnGet() => ThisOption = MaintenanceOption.Office;

    public async Task<IActionResult> OnPostAsync(
        [FromServices] IOfficeService service,
        [FromServices] IValidator<OfficeCreateDto> validator)
    {
        ThisOption = MaintenanceOption.Office;
        return await DoPost(service, validator, Item);
    }
}
