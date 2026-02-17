using AirWeb.AppServices.Compliance.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;
using AirWeb.AppServices.Compliance.Enforcement;
using AirWeb.AppServices.Compliance.Enforcement.Permissions;
using AirWeb.WebApp.Models;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(CompliancePolicies.ComplianceStaff))]
public class LinkedEventsModel(ICaseFileService service) : PageModel
{
    [FromRoute]
    public int Id { get; set; } // Case File ID

    public IEnumerable<ComplianceWorkSearchResultDto> LinkedComplianceEvents { get; private set; } = [];
    public IEnumerable<ComplianceWorkSearchResultDto> AvailableComplianceEvents { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        if (Id == 0) return RedirectToPage("Index");

        var item = await service.FindSummaryAsync(Id, token);
        if (item is null) return NotFound();
        if (item.IsClosed) return BadRequest();
        if (!User.CanEditCaseFile(item)) return Forbid();

        LinkedComplianceEvents = await service.GetLinkedEventsAsync(Id, token);
        AvailableComplianceEvents = await service.GetAvailableEventsAsync((FacilityId)item.FacilityId,
            LinkedComplianceEvents, token);
        return Page();
    }

    public async Task<IActionResult> OnPostLinkEventAsync(int eventId, CancellationToken token)
    {
        if (Id == 0 || eventId == 0) return BadRequest();
        var item = await service.FindSummaryAsync(Id, token);
        if (item is null || item.IsClosed || !User.CanEditCaseFile(item)) return BadRequest();

        var result = await service.LinkComplianceEventAsync(Id, eventId, token);

        if (result)
        {
            TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success,
                $"Compliance Event #{eventId} successfully linked.");
        }
        else
        {
            TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning,
                $"There was an error linking Compliance Event #{eventId}.");
        }

        return RedirectToPage("LinkedEvents", null, new { Id });
    }

    public async Task<IActionResult> OnPostUnlinkEventAsync(int eventId, CancellationToken token)
    {
        if (Id == 0 || eventId == 0) return BadRequest();
        var item = await service.FindSummaryAsync(Id, token);
        if (item is null || item.IsClosed || !User.CanEditCaseFile(item)) return BadRequest();

        var result = await service.UnLinkComplianceEventAsync(Id, eventId, token: token);

        if (result)
        {
            TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success,
                $"Compliance Event #{eventId} successfully unlinked.");
        }
        else
        {
            TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning,
                $"There was an error unlinking Compliance Event #{eventId}.");
        }

        return RedirectToPage("LinkedEvents", null, new { Id });
    }
}
