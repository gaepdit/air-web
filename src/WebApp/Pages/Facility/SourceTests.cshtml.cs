using AirWeb.AppServices.Core.AuthorizationServices;
using IaipDataService.Facilities;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace AirWeb.WebApp.Pages.Facility;

[Authorize(Policy = nameof(Policies.Staff))]
public class SourceTestsModel(IFacilityService facilityService, ISourceTestService sourceTestService) : PageModel
{
    [FromRoute]
    public string? Id { get; set; }

    public IaipDataService.Facilities.Facility? Facility { get; private set; }
    public IList<SourceTestSummary> SourceTests { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync(CancellationToken token = default)
    {
        if (string.IsNullOrEmpty(Id)) return RedirectToPage("Index");

        if (!FacilityId.TryParse(Id, out var facilityId)) return NotFound("Facility ID not found.");

        if (facilityId.FormattedId != Id) return RedirectToPage(new { id = facilityId });

        Facility = await facilityService.FindFacilityAsync(facilityId, token: token);
        if (Facility is null) return NotFound("Facility ID not found.");

        SourceTests = (await sourceTestService.GetSourceTestsForFacilityAsync(facilityId))
            .ToList();

        return Page();
    }
}
