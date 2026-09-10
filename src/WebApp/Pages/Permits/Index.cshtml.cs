using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using IaipDataService.Permits;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace AirWeb.WebApp.Pages.Permits;

[AllowAnonymous]
public class PermitSearchIndex(IFacilityService service) : PageModel
{
    [Display(Name = "Facility ID/AIRS Number")]
    [StringLength(9)]
    public string? Id { get; set; }

    [Display(Name = "Facility Name")]
    [StringLength(100)]
    public string? Name { get; set; }

    public bool ShowResults { get; private set; }

    public string FacilitiesAsJson => JsonSerializer.Serialize(Facilities, SerializationDefaults.Options);
    public IReadOnlyCollection<FacilityList> Facilities { get; private set; } = null!;
    public IPaginatedResult<PermitSummary> SearchResults { get; private set; } = null!;
    public PaginatedResultsDisplay ResultsDisplay => new(RouteValues, SearchResults);
    public Dictionary<string, string?> RouteValues => new() { { nameof(Id), Id }, { nameof(Name), Name } };

    public async Task OnGetAsync(CancellationToken token = default) => Facilities = await service.GetListAsync(token);

    public async Task OnGetSearchAsync(string? id, string? name, [FromQuery] int p = 1,
        CancellationToken token = default)
    {
        Id = id;
        Name = name;
        Facilities = await service.GetListAsync(token);

        if (Id != null)
        {
            if (!FacilityIdRegex.IsValidSearchFormat(Id))
                ModelState.AddModelError(nameof(Id), FacilityId.FacilityIdFormatError);
            if (!await service.ExistsAsync((FacilityId)Id))
                ModelState.AddModelError(nameof(Id), FacilityId.FacilityNotExistsShortError);
        }

        if (!ModelState.IsValid) return;

        var paging = PaginationDefaults.DefaultSearch(p);
        var permits = await service.GetPermitListAsync(Id, Name, paging.Skip, paging.Take, token);
        var permitCount = await service.CountPermitsAsync(Id, Name, token);
        SearchResults = new PaginatedResult<PermitSummary>(permits, permitCount, paging);
        ShowResults = true;
    }
}
