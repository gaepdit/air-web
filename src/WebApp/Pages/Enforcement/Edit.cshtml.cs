using AirWeb.AppServices.Compliance.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.Enforcement;
using AirWeb.AppServices.Compliance.Enforcement.CaseFileCommand;
using AirWeb.AppServices.Compliance.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Compliance.Enforcement.Permissions;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.Domain.Compliance.AppRoles;
using AirWeb.Domain.Compliance.EnforcementEntities.ViolationTypes;
using AirWeb.WebApp.Models;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(CompliancePolicies.ComplianceStaff))]
public class EditModel(
    ICaseFileService caseFileService,
    IStaffService staffService,
    IValidator<CaseFileUpdateDto> validator) : PageModel, ISubmitCancelButtons
{
    [FromRoute]
    public int Id { get; set; } // Case File ID

    [BindProperty]
    public CaseFileUpdateDto Item { get; set; } = null!;

    public CaseFileViewDto ItemView { get; private set; } = null!;
    public SelectList StaffSelectList { get; private set; } = null!;
    public SelectList ViolationTypeSelectList { get; private set; } = null!;

    // Form buttons
    public string SubmitText => "Save Changes";
    public string CancelRoute => "Details";
    public string RouteId => Id.ToString();

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        if (Id == 0) return RedirectToPage("Index");

        var itemView = await caseFileService.FindDetailedAsync(Id, token);
        if (itemView is null) return NotFound();
        if (!User.CanEditCaseFile(itemView)) return Forbid();

        ItemView = itemView;
        Item = new CaseFileUpdateDto(ItemView);

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var itemView = await caseFileService.FindDetailedAsync(Id, token);
        if (itemView is null || !User.CanEditCaseFile(itemView)) return BadRequest();
        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            ItemView = itemView;
            await PopulateSelectListsAsync();
            return Page();
        }

        await caseFileService.UpdateAsync(Id, Item, token);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "Enforcement successfully updated.");
        return RedirectToPage("Details", new { Id });
    }

    private async Task PopulateSelectListsAsync()
    {
        StaffSelectList = (await staffService.GetUsersInRoleAsync(ComplianceRole.ComplianceStaffRole)).ToSelectList();
        ViolationTypeSelectList = new SelectList(ViolationTypeData.GetCurrent(),
            nameof(ViolationType.Code), nameof(ViolationType.Display),
            null, nameof(ViolationType.SeverityCode));
    }
}
