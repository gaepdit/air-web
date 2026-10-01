using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using IaipDataService.Permits;

namespace AirWeb.WebApp.Pages.Permits;

[AllowAnonymous]
public class FacilityPermitsIndex(IPermitService permitService, IFacilityService facilityService) : PageModel
{
    [FromRoute]
    public string? Id { get; set; }

    public IaipDataService.Facilities.Facility? Facility { get; private set; }

    public IPaginatedResult<PermitSummary> SearchResults { get; private set; } = null!;

    public PaginatedResultsDisplay ResultsDisplay => new(RouteValues, SearchResults)
        { SearchHandler = "", SearchFragment = "" };

    public Dictionary<string, string?> RouteValues => new() { { nameof(Id), Id } };

    public async Task<IActionResult> OnGetAsync([FromQuery] int p = 1, CancellationToken token = default)
    {
        if (string.IsNullOrEmpty(Id)) return RedirectToPage("Index");
        if (!FacilityId.TryParse(Id, out var facilityId)) return NotFound("Facility ID not found.");
        if (facilityId.FormattedId != Id) return RedirectToPage(new { id = facilityId });

        Facility = await facilityService.FindFacilityAsync(facilityId, token: token);
        if (Facility is null) return NotFound("Facility ID not found.");

        var permitCount = await permitService.CountPermitsAsync(facilityId);
        var paging = PaginationDefaults.DefaultSearch(p);
        var permits = permitCount > 0
            ? await permitService.SearchPermitsAsync(facilityId, paging.Skip, paging.Take, token)
            : [];

        SearchResults = new PaginatedResult<PermitSummary>(permits, permitCount, paging);
        return Page();
    }
}
