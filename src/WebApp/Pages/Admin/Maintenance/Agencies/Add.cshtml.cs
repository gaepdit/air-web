using AirWeb.AppServices.Sbeap.Agencies;
using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.Agencies;

[Authorize(Policy = nameof(SbeapPolicies.SbeapSiteMaintainer))]
public class AddModel : AddBase
{
    [BindProperty]
    public AgencyCreateDto Item { get; set; } = null!;

    public void OnGet()
    {
        ThisOption = MaintenanceOption.SbeapAgency;
        Item = new AgencyCreateDto();
    }

    public async Task<IActionResult> OnPostAsync(
        [FromServices] IAgencyService service,
        [FromServices] IValidator<AgencyCreateDto> validator)
    {
        ThisOption = MaintenanceOption.SbeapAgency;
        return await DoPostAsync(service, validator, Item);
    }
}
