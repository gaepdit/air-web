using AirWeb.AppServices.Sbeap.Agencies;
using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.Agencies;

[Authorize(Policy = nameof(SbeapPolicies.SbeapSiteMaintainer))]
public class EditModel(IAgencyService service, IValidator<AgencyUpdateDto> validator) : EditBase
{
    [BindProperty]
    public AgencyUpdateDto Item { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        ThisOption = MaintenanceOption.Agency;

        if (Id is null) return RedirectToPage("Index");
        var originalItem = await service.FindForUpdateAsync(Id.Value);
        if (originalItem is null) return NotFound();

        OriginalName = originalItem.Name;
        Item = originalItem;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ThisOption = MaintenanceOption.Agency;
        return await DoPostAsync(service, validator, Item);
    }
}
