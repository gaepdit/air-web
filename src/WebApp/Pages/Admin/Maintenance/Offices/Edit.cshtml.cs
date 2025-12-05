using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Lookups.Offices;
using AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.Offices;

[Authorize(Policy = nameof(Policies.SiteMaintainer))]
public class EditModel(IOfficeService service, IValidator<OfficeUpdateDto> validator) : EditBase
{
    [BindProperty]
    public OfficeUpdateDto Item { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        ThisOption = MaintenanceOption.Office;

        if (Id is null) return RedirectToPage("Index");
        var originalItem = await service.FindForUpdateAsync(Id.Value);
        if (originalItem is null) return NotFound();

        OriginalName = originalItem.Name;
        Item = originalItem;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ThisOption = MaintenanceOption.Office;
        return await DoPostAsync(service, validator, Item);
    }
}
