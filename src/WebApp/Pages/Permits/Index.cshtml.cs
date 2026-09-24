using AirWeb.AppServices.Core.DataAttributes;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
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
    [StringLength(9)]
    [Required(ErrorMessage = FacilityId.FacilityIdBlankError)]
    [RequiredNoLabel]
    public string? Id { get; set; }

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
        { nameof(Id), Id },
        { nameof(Spec.Name), Spec.Name },
        { nameof(Spec.Permit), Spec.Permit },
        { nameof(Spec.DateFrom), Spec.DateFrom?.ToString("yyyy-MM-dd") },
        { nameof(Spec.DateTo), Spec.DateTo?.ToString("yyyy-MM-dd") },
    };

    public async Task OnGetAsync(CancellationToken token = default) =>
        Facilities = await facilityService.GetListAsync(token);

    public async Task<IActionResult> OnGetFacilityAsync(string id, [FromQuery] int p = 1,
        CancellationToken token = default)
    {
        Id = FacilityId.TryFormat(id);
        if (Id != id) return RedirectToPage(new { handler = "Facility", Id });

        Facilities = await facilityService.GetListAsync(token);
        ModelState.Clear();

        if (Id == null)
        {
            ModelState.AddModelError(nameof(Id), FacilityId.FacilityIdBlankError);
            return Page();
        }

        if (!FacilityIdRegex.IsValidSearchFormat(Id))
        {
            ModelState.AddModelError(nameof(Id), FacilityId.FacilityIdFormatError);
            return Page();
        }

        // This is a public page. If the user enters a Facility ID that matches the correct format, but does not
        // comply with the business rules (e.g., "000-00001" or "002-00001"), the most comprehensible response is that
        // a facility with that ID doesn't exist. I.e., don't say that the ID format is invalid.
        if (!FacilityIdRegex.IsValidStandardFormat(Id))
        {
            ModelState.AddModelError(nameof(Id), FacilityId.FacilityNotExistsShortError);
            return Page();
        }

        if (!ModelState.IsValid) return Page();

        var facilityId = (FacilityId)Id;

        if (!await facilityService.ExistsAsync(facilityId))
        {
            ModelState.AddModelError(nameof(Id), FacilityId.FacilityNotExistsShortError);
            return Page();
        }

        var paging = PaginationDefaults.DefaultSearch(p);
        var permits = await service.SearchPermitsAsync(facilityId, paging.Skip, paging.Take, token);
        var permitCount = await service.CountPermitsAsync(facilityId);
        SearchResults = new PaginatedResult<PermitSummary>(permits, permitCount, paging);
        ShowResults = true;
        SearchHandler = "Facility";
        Spec = new PermitSearchDto();
        return Page();
    }

    public async Task OnGetSearchAsync(PermitSearchDto spec, [FromQuery] int p = 1, CancellationToken token = default)
    {
        Facilities = await facilityService.GetListAsync(token);
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
