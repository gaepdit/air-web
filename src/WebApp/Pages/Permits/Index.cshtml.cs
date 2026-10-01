using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using IaipDataService.Facilities.Models;
using IaipDataService.Permits;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace AirWeb.WebApp.Pages.Permits;

[AllowAnonymous]
public class PermitSearchIndex(
    IPermitService service,
    IFacilityService facilityService,
    IValidator<PermitSearchDto> validator) : PageModel
{
    // Facility ID search form
    [BindProperty]
    [StringLength(9)]
    [Required(ErrorMessage = FacilityId.FacilityIdBlankError)]
    public string? FindId { get; set; }

    // Permit details search form
    public PermitSearchDto Spec { get; private set; } = null!;

    // Page properties and data
    public bool ShowResults { get; private set; }
    public string SearchHandler { get; private set; } = string.Empty;

    public string FacilitiesAsJson => JsonSerializer.Serialize(Facilities, SerializationDefaults.Options);
    public IReadOnlyCollection<FacilityList> Facilities { get; private set; } = null!;
    public IPaginatedResult<PermitSummary> SearchResults { get; private set; } = null!;
    public PaginatedResultsDisplay ResultsDisplay => new(RouteValues, SearchHandler, SearchResults);

    public Dictionary<string, string?> RouteValues => new()
    {
        { nameof(Spec.Name), Spec.Name },
        { nameof(Spec.Permit), Spec.Permit },
        { nameof(Spec.DateFrom), Spec.DateFrom?.ToString("yyyy-MM-dd") },
        { nameof(Spec.DateTo), Spec.DateTo?.ToString("yyyy-MM-dd") },
    };

    public async Task OnGetAsync(CancellationToken token = default) =>
        Facilities = await facilityService.GetListAsync(token: token);

    public async Task<IActionResult> OnPostAsync(CancellationToken token = default)
    {
        ModelState.Clear();

        if (FindId == null)
            ModelState.AddModelError(nameof(FindId), FacilityId.FacilityIdBlankError);
        else if (!FacilityIdRegex.IsValidSearchFormat(FindId))
            ModelState.AddModelError(nameof(FindId), FacilityId.FacilityIdFormatError);
        else if (!FacilityIdRegex.IsValidStandardFormat(FindId))
            // This is a public page. If the user enters a Facility ID that matches the correct format, but does not
            // comply with the business rules (e.g., "000-00001" or "002-00001"), the most comprehensible response is that
            // a facility with that ID doesn't exist. I.e., don't just say that the ID format is invalid.
            ModelState.AddModelError(nameof(FindId), FacilityId.FacilityNotExistsShortError);
        else
        {
            var facilityId = (FacilityId)FindId;
            if (!await facilityService.ExistsAsync(facilityId))
                ModelState.AddModelError(nameof(FindId), FacilityId.FacilityNotExistsShortError);
            else if (await service.CountPermitsAsync(facilityId) == 0)
                ModelState.AddModelError(nameof(FindId), "No permits were found for that facility. ");
        }

        if (ModelState.IsValid) return RedirectToPage("Facility", routeValues: new { id = FindId });

        SearchHandler = "Facility";
        Facilities = await facilityService.GetListAsync(token: token);
        return Page();
    }

    public async Task OnGetSearchAsync(PermitSearchDto spec, [FromQuery] int p = 1, CancellationToken token = default)
    {
        Facilities = await facilityService.GetListAsync(token: token);
        await validator.ApplyValidationAsync(spec, ModelState);
        Spec = spec.TrimAll();
        if (!ModelState.IsValid) return;

        var paging = PaginationDefaults.DefaultSearch(p);
        var permitSearchTask = service.SearchPermitsAsync(Spec, paging.Skip, paging.Take);
        var permitCountTask = service.CountPermitsAsync(Spec);

        SearchResults = new PaginatedResult<PermitSummary>(await permitSearchTask, await permitCountTask, paging);
        ShowResults = true;
        SearchHandler = "Search";
    }
}
