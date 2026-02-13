using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.AppRoles;
using AirWeb.WebApp.Models;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Compliance.FCE;

[Authorize(Policy = nameof(CompliancePolicies.ComplianceStaff))]
public class EditModel(IFceService fceService, IStaffService staffService) : PageModel, ISubmitCancelButtons
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty]
    public FceUpdateDto Item { get; set; } = null!;

    public FceSummaryDto ItemView { get; private set; } = null!;
    public SelectList StaffSelectList { get; private set; } = null!;

    // Form buttons
    public string SubmitText => "Save Changes";
    public string CancelRoute => "Details";
    public string RouteId => Id.ToString();

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        if (Id == 0) return RedirectToPage("Index");

        var itemView = await fceService.FindSummaryAsync(Id, token);
        if (itemView is null) return NotFound();
        if (!User.CanEdit(itemView)) return Forbid();

        ItemView = itemView;
        Item = new FceUpdateDto(ItemView);

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var itemView = await fceService.FindSummaryAsync(Id, token);
        if (itemView is null || !User.CanEdit(itemView)) return BadRequest();

        if (!ModelState.IsValid)
        {
            ItemView = itemView;
            await PopulateSelectListsAsync();
            return Page();
        }

        var result = await fceService.UpdateAsync(Id, Item, token);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, "FCE successfully updated.");
        if (result.HasWarning) TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, result.WarningMessage);

        return RedirectToPage("Details", new { Id });
    }

    // FUTURE: Allow for editing an FCE previously reviewed by a currently inactive user.
    private async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetUsersInRoleAsync(ComplianceRole.ComplianceStaffRole)).ToSelectList();
}
