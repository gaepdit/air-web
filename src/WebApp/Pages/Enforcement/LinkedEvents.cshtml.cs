using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Permissions;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class LinkedEventsModel(ICaseFileService service) : PageModel
{
    [FromRoute]
    public int Id { get; set; } // Case File ID

    public IEnumerable<WorkEntrySearchResultDto> LinkedComplianceEvents { get; private set; } = [];
    public IEnumerable<WorkEntrySearchResultDto> AvailableComplianceEvents { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        if (Id == 0) return RedirectToPage("Index");

        var item = await service.FindSummaryAsync(Id, token);
        if (item is null) return NotFound();
        if (item.IsClosed) return BadRequest();
        if (!User.CanEdit(item)) return Forbid();

        LinkedComplianceEvents = await service.GetLinkedEventsAsync(Id, token);
        AvailableComplianceEvents = await service.GetAvailableEventsAsync((FacilityId)item.FacilityId,
            LinkedComplianceEvents, token);
        return Page();
    }

    public async Task<IActionResult> OnPostLinkEventAsync(int entryId, CancellationToken token)
    {
        if (Id == 0 || entryId == 0) return BadRequest();
        var item = await service.FindSummaryAsync(Id, token);
        if (item is null || item.IsClosed || !User.CanEdit(item)) return BadRequest();

        var result = await service.LinkComplianceEvent(Id, entryId, token);

        if (result)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success,
                $"Compliance Event #{entryId} successfully linked.");
        }
        else
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                $"There was an error linking Compliance Event #{entryId}.");
        }

        return RedirectToPage("LinkedEvents", null, new { Id });
    }

    public async Task<IActionResult> OnPostUnlinkEventAsync(int entryId, CancellationToken token)
    {
        if (Id == 0 || entryId == 0) return BadRequest();
        var item = await service.FindSummaryAsync(Id, token);
        if (item is null || item.IsClosed || !User.CanEdit(item)) return BadRequest();

        var result = await service.UnLinkComplianceEvent(Id, entryId, token);

        if (result)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success,
                $"Compliance Event #{entryId} successfully unlinked.");
        }
        else
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                $"There was an error unlinking Compliance Event #{entryId}.");
        }

        return RedirectToPage("LinkedEvents", null, new { Id });
    }
}
