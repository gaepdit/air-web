using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.AppServices.Sbeap.Customers;
using AirWeb.AppServices.Sbeap.Customers.Dto;
using AirWeb.AppServices.Sbeap.Customers.Permissions;
using AirWeb.WebApp.Models;

namespace AirWeb.WebApp.Pages.SBEAP.Customers;

[Authorize(Policy = nameof(SbeapPolicies.SbeapAdmin))]
public class RestoreModel(ICustomerService service, IAuthorizationService authorization)
    : PageModel
{
    // Properties
    [BindProperty]
    public Guid Id { get; set; }

    public CustomerViewDto Item { get; private set; } = null!;

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");
        var item = await service.FindAsync(id.Value);
        if (item is null) return NotFound();
        if (!await UserCanManageDeletionsAsync(item)) return Forbid();

        if (!item.IsDeleted)
        {
            TempData.AddDisplayMessage(DisplayMessage.AlertContext.Info, "Customer is not deleted.");
            return RedirectToPage("Details", new { id });
        }

        Id = id.Value;
        Item = item;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var item = await service.FindAsync(Id);
        if (item is null) return BadRequest();
        if (!await UserCanManageDeletionsAsync(item)) return BadRequest();

        if (!item.IsDeleted)
        {
            TempData.AddDisplayMessage(DisplayMessage.AlertContext.Info, "Customer is not deleted.");
            return RedirectToPage("Details", new { Id });
        }

        if (!ModelState.IsValid)
        {
            Item = item;
            return Page();
        }

        await service.RestoreAsync(Id);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "Customer successfully restored.");
        return RedirectToPage("Details", new { Id });
    }

    private async Task<bool> UserCanManageDeletionsAsync(CustomerViewDto item) =>
        (await authorization.AuthorizeAsync(User, item, CustomerOperation.ManageDeletions)).Succeeded;
}
