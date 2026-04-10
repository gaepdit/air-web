using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.AppServices.Sbeap.Customers;
using AirWeb.AppServices.Sbeap.Customers.Dto;
using AirWeb.Domain.Core.Data;
using AirWeb.WebApp.Models;
using FluentValidation;

namespace AirWeb.WebApp.Pages.SBEAP.Customers;

[Authorize(Policy = nameof(SbeapPolicies.SbeapStaff))]
public class AddContactModel(
    ICustomerService service,
    IValidator<ContactCreateDto> validator,
    IAuthorizationService authorization)
    : PageModel
{
    // Properties
    [BindProperty]
    public ContactCreateDto NewContact { get; set; } = null!;

    [TempData]
    public Guid HighlightId { get; set; }

    public CustomerSearchResultDto Customer { get; private set; } = null!;

    // Select lists
    public static SelectList StatesSelectList => new(CommonData.States);

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");
        var customer = await service.FindBasicInfoAsync(id.Value);
        if (customer is null) return NotFound();

        Customer = customer;
        NewContact = new ContactCreateDto(id.Value);

        if (!Customer.IsDeleted) return Page();
        if (!await UserCanManageDeletionsAsync()) return NotFound();
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Info, "Cannot add a contact to a deleted customer.");
        return RedirectToPage("Details", new { id });
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");
        if (NewContact.CustomerId != id) return BadRequest();

        var customer = await service.FindBasicInfoAsync(id.Value);
        if (customer is null) return BadRequest();

        Customer = customer;
        if (Customer.IsDeleted) return Forbid();

        await validator.ApplyValidationAsync(NewContact, ModelState);
        if (!ModelState.IsValid) return Page();

        HighlightId = await service.AddContactAsync(NewContact);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "New Contact successfully added.");
        return RedirectToPage("Details", null, new { id }, "contacts");
    }

    private async Task<bool> UserCanManageDeletionsAsync() =>
        (await authorization.AuthorizeAsync(User, nameof(SbeapPolicies.SbeapAdmin))).Succeeded;
}
