using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.AppServices.Lookups.Offices;
using AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.Offices;

[Authorize(Policy = nameof(Policies.SiteMaintainer))]
public class AddModel : AddBase
{
    [BindProperty]
    public OfficeCreateDto Item { get; set; } = null!;

    public void OnGet()
    {
        ThisOption = MaintenanceOption.Office;
        Item = new OfficeCreateDto();
    }

    public async Task<IActionResult> OnPostAsync(
        [FromServices] IOfficeService service,
        [FromServices] IValidator<OfficeCreateDto> validator)
    {
        ThisOption = MaintenanceOption.Office;
        return await DoPostAsync(service, validator, Item);
    }
}
