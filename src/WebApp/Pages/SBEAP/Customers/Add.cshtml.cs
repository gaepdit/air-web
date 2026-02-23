using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.AppServices.Sbeap.Customers;
using AirWeb.AppServices.Sbeap.Customers.Dto;
using AirWeb.Domain.Core.Data;
using AirWeb.WebApp.Models;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.SBEAP.Customers;

[Authorize(Policy = nameof(SbeapPolicies.SbeapStaff))]
public class AddModel(ICustomerService service, IValidator<CustomerCreateDto> validator)
    : PageModel
{
    // Properties
    [BindProperty]
    public CustomerCreateDto Item { get; set; } = null!;

    // Select lists
    public static SelectList StatesSelectList => new(CommonData.States);
    public static SelectList CountiesSelectList => new(CommonData.Counties);
    public SelectList SicSelectList { get; private set; } = null!;

    // Methods
    public IActionResult OnGet()
    {
        PopulateSelectLists();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await validator.ApplyValidationAsync(Item, ModelState);
        if (!ModelState.IsValid)
        {
            PopulateSelectLists();
            return Page();
        }

        var id = await service.CreateAsync(Item);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "New Customer successfully added.");
        return RedirectToPage("Details", new { id });
    }

    private void PopulateSelectLists() => SicSelectList = SicCodes.ActiveListItems.ToSelectList();
}
