using AirWeb.AppServices.Sbeap.ActionItemTypes;
using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.ActionItemTypes;

[Authorize(Policy = nameof(SbeapPolicies.SbeapSiteMaintainer))]
public class EditModel(IActionItemTypeService service, IValidator<ActionItemTypeUpdateDto> validator) : EditBase
{
    [BindProperty]
    public ActionItemTypeUpdateDto Item { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        ThisOption = MaintenanceOption.ActionItemType;

        if (Id is null) return RedirectToPage("Index");
        var originalItem = await service.FindForUpdateAsync(Id.Value);
        if (originalItem is null) return NotFound();

        OriginalName = originalItem.Name;
        Item = originalItem;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ThisOption = MaintenanceOption.ActionItemType;
        return await DoPostAsync(service, validator, Item);
    }
}
