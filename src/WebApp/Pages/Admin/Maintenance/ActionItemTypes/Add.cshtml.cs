using AirWeb.AppServices.Sbeap.ActionItemTypes;
using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.ActionItemTypes;

[Authorize(Policy = nameof(SbeapPolicies.SbeapSiteMaintainer))]
public class AddModel : AddBase
{
    [BindProperty]
    public ActionItemTypeCreateDto Item { get; set; } = null!;

    public void OnGet()
    {
        ThisOption = MaintenanceOption.ActionItemType;
        Item = new ActionItemTypeCreateDto();
    }

    public async Task<IActionResult> OnPostAsync(
        [FromServices] IActionItemTypeService service,
        [FromServices] IValidator<ActionItemTypeCreateDto> validator)
    {
        ThisOption = MaintenanceOption.ActionItemType;
        return await DoPostAsync(service, validator, Item);
    }
}
