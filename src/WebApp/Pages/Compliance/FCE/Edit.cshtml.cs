using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Compliance.FCE;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class EditModel(IFceService fceService, IStaffService staffService) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty]
    public FceUpdateDto Item { get; set; } = null!;

    public FceSummaryDto ItemView { get; private set; } = null!;
    public SelectList StaffSelectList { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");

        var item = await fceService.FindForUpdateAsync(Id);
        if (item is null) return NotFound();
        if (!User.CanEdit(item)) return Forbid();

        var itemView = await fceService.FindSummaryAsync(Id);
        if (itemView is null) return BadRequest();

        Item = item;
        ItemView = itemView;

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var original = await fceService.FindForUpdateAsync(Id, token);
        if (original is null || !User.CanEdit(original)) return BadRequest();

        if (!ModelState.IsValid)
        {
            var itemView = await fceService.FindSummaryAsync(Id, token);
            if (itemView is null) return BadRequest();
            ItemView = itemView;

            await PopulateSelectListsAsync();
            return Page();
        }

        await fceService.UpdateAsync(Id, Item, token);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "FCE successfully updated.");
        return RedirectToPage("Details", new { Id });
    }

    // FUTURE: Allow for editing an FCE previously reviewed by a currently inactive user.
    private async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();
}
