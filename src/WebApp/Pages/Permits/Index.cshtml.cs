using AirWeb.AppServices.Core.DataAttributes;
using AirWeb.AppServices.Core.Utilities;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.AppLibrary.DataAttributes;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using IaipDataService.Permits;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace AirWeb.WebApp.Pages.Permits;

[AllowAnonymous]
public class PermitSearchIndex(IPermitService service, IFacilityService facilityService) : PageModel
{
    // Facility ID search form
    [StringLength(9)]
    [Required(ErrorMessage = FacilityId.FacilityIdBlankError)]
    [RequiredNoLabel]
    public string? Id { get; set; }

    // Permit details search form
    [Display(Name = "Facility Name")]
    [StringLength(100)]
    public string? Name { get; private set; }

    [Display(Name = "Permit Number/SIC Code")]
    [StringLength(20)]
    public string? Permit { get; private set; }

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [MaxDate]
    public DateOnly? DateFrom { get; private set; }

    [Display(Name = "Until")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [MaxDate]
    public DateOnly? DateTo { get; private set; }

    // Page properties and data
    public bool ShowResults { get; private set; }
    public string HighlightForm { get; private set; } = string.Empty;

    public string FacilitiesAsJson => JsonSerializer.Serialize(Facilities, SerializationDefaults.Options);
    public IReadOnlyCollection<FacilityList> Facilities { get; private set; } = null!;
    public IPaginatedResult<PermitSummary> SearchResults { get; private set; } = null!;
    public PaginatedResultsDisplay ResultsDisplay => new(RouteValues, SearchResults);

    public Dictionary<string, string?> RouteValues => new()
    {
        { nameof(Id), Id },
        { nameof(Name), Name },
        { nameof(Permit), Permit },
        { nameof(DateFrom), DateFrom?.ToString("d") },
        { nameof(DateTo), DateTo?.ToString("d") },
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
        var permits = await service.SearchPermitsAsync(facilityId, paging.Skip, paging.Take);
        var permitCount = await service.CountPermitsAsync(facilityId);
        SearchResults = new PaginatedResult<PermitSummary>(permits, permitCount, paging);
        ShowResults = true;
        HighlightForm = "Facility";
        return Page();
    }

    public async Task OnGetSearchAsync(string? name, string? permit, DateOnly? dateFrom, DateOnly? dateTo,
        [FromQuery] int p = 1, CancellationToken token = default)
    {
        Name = name;
        Permit = permit;
        DateTo = dateTo;
        DateFrom = dateFrom;
        Facilities = await facilityService.GetListAsync(token);

        var paging = PaginationDefaults.DefaultSearch(p);
        var permits = await service.SearchPermitsAsync(Name, Permit, dateFrom, dateTo, paging.Skip, paging.Take);
        var permitCount = await service.CountPermitsAsync(Name, Permit, dateFrom, dateTo);
        SearchResults = new PaginatedResult<PermitSummary>(permits, permitCount, paging);
        ShowResults = true;
        HighlightForm = "Search";
    }
}
