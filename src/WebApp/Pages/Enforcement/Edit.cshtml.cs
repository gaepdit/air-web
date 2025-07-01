using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.CaseFileCommand;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.WebApp.Models;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class EditModel(
    ICaseFileService caseFileService,
    IStaffService staffService,
    IValidator<CaseFileUpdateDto> validator) : PageModel, ISubmitCancelButtons
{
    [FromRoute]
    public int Id { get; set; } // Case File ID

    [BindProperty]
    public CaseFileUpdateDto Item { get; set; } = null!;

    public CaseFileSummaryDto ItemView { get; private set; } = null!;
    public SelectList StaffSelectList { get; private set; } = null!;

    // Form buttons
    public string SubmitText => "Save Changes";
    public string CancelRoute => "Details";
    public string RouteId => Id.ToString();

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        if (Id == 0) return RedirectToPage("Index");

        var itemView = await caseFileService.FindSummaryAsync(Id, token);
        if (itemView is null) return NotFound();
        if (!User.CanEditCaseFile(itemView)) return Forbid();

        ItemView = itemView;
        Item = new CaseFileUpdateDto(ItemView);

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var itemView = await caseFileService.FindSummaryAsync(Id, token);
        if (itemView is null || !User.CanEditCaseFile(itemView)) return BadRequest();
        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            ItemView = itemView;
            await PopulateSelectListsAsync();
            return Page();
        }

        await caseFileService.UpdateAsync(Id, Item, token);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Enforcement successfully updated.");
        return RedirectToPage("Details", new { Id });
    }

    private async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();
}
