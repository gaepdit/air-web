using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.AppServices.Sbeap.Customers;
using AirWeb.AppServices.Sbeap.Customers.Dto;
using AirWeb.AppServices.Sbeap.Customers.Permissions;
using AirWeb.Domain.Core.Data;
using AirWeb.WebApp.Models;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.SBEAP.Customers;

[Authorize(Policy = nameof(SbeapPolicies.SbeapStaff))]
public class EditModel(
    ICustomerService service,
    IValidator<CustomerUpdateDto> validator,
    IAuthorizationService authorization)
    : PageModel
{
    // Properties

    [FromRoute]
    public Guid Id { get; set; }

    [BindProperty]
    public CustomerUpdateDto Item { get; set; } = null!;

    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    // Select lists
    public static SelectList StatesSelectList => new(CommonData.States);
    public static SelectList CountiesSelectList => new(CommonData.Counties);
    public SelectList SicSelectList { get; private set; } = null!;

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");
        var item = await service.FindForUpdateAsync(id.Value);
        if (item is null) return NotFound();

        await SetPermissionsAsync(item);

        if (UserCan[CustomerOperation.Edit])
        {
            Id = id.Value;
            Item = item;
            PopulateSelectLists();
            return Page();
        }

        if (!UserCan[CustomerOperation.ManageDeletions]) return NotFound();

        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Info, "Cannot edit a deleted customer.");
        return RedirectToPage("Details", new { id });
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var originalItem = await service.FindForUpdateAsync(Id);
        if (originalItem is null) return BadRequest();
        await SetPermissionsAsync(originalItem);
        if (!UserCan[CustomerOperation.Edit]) return BadRequest();

        await validator.ApplyValidationAsync(Item, ModelState);
        if (!ModelState.IsValid)
        {
            PopulateSelectLists();
            return Page();
        }

        await service.UpdateAsync(Id, Item);

        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "Customer successfully updated.");
        return RedirectToPage("Details", new { Id });
    }

    private void PopulateSelectLists() => SicSelectList = SicCodes.ActiveListItems.ToSelectList();

    private async Task SetPermissionsAsync(CustomerUpdateDto item)
    {
        foreach (var operation in CustomerOperation.AllOperations)
            UserCan[operation] = (await authorization.AuthorizeAsync(User, item, operation)).Succeeded;
    }
}
