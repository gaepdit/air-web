using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.AppServices.Sbeap.Cases;
using AirWeb.AppServices.Sbeap.Cases.Dto;
using AirWeb.AppServices.Sbeap.Customers;
using AirWeb.AppServices.Sbeap.Customers.Dto;
using AirWeb.AppServices.Sbeap.Customers.Permissions;
using AirWeb.WebApp.Models;

namespace AirWeb.WebApp.Pages.SBEAP.Customers;

[Authorize(Policy = nameof(SbeapPolicies.SbeapStaff))]
public class DetailsModel(ICustomerService customers, ICaseworkService cases, IAuthorizationService authorization)
    : PageModel
{
    // Properties
    [BindProperty]
    public CaseworkCreateDto NewCase { get; set; } = null!;

    [TempData]
    public Guid HighlightId { get; set; }

    public CustomerViewDto Item { get; private set; } = null!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");
        var item = await customers.FindAsync(id.Value, await ShowDeletedCasesAsync());
        if (item is null) return NotFound();

        await SetPermissionsAsync(item);
        if (item.IsDeleted && !UserCan[CustomerOperation.ManageDeletions])
            return NotFound();

        Item = item;
        NewCase = new CaseworkCreateDto(id.Value);
        return Page();
    }

    /// <summary>
    /// Post is used to add a new Case for this Customer
    /// </summary>
    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");
        if (NewCase.CustomerId != id) return BadRequest();

        var item = await customers.FindAsync(id.Value);
        if (item is null || item.IsDeleted) return BadRequest();

        await SetPermissionsAsync(item);
        if (!UserCan[CustomerOperation.Edit]) return Forbid();

        if (!ModelState.IsValid)
        {
            Item = item;
            return Page();
        }

        var caseId = await cases.CreateAsync(NewCase);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "New Case successfully added.");
        return RedirectToPage("../Cases/Details", new { id = caseId });
    }

    private async Task SetPermissionsAsync(CustomerViewDto item)
    {
        foreach (var operation in CustomerOperation.AllOperations)
            UserCan[operation] = (await authorization.AuthorizeAsync(User, item, operation)).Succeeded;
    }

    private async Task<bool> ShowDeletedCasesAsync() =>
        (await authorization.AuthorizeAsync(User, nameof(SbeapPolicies.SbeapAdmin))).Succeeded;
}
