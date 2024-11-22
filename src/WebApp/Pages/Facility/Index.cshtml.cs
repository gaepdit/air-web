using AirWeb.AppServices.Permissions;
using IaipDataService.Facilities;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.WebApp.Pages.Facility;

[Authorize(Policy = nameof(Policies.Staff))]
public class IndexModel(IFacilityService service) : PageModel
{
    public ReadOnlyDictionary<FacilityId, string> Facilities { get; private set; } = null!;

    [TempData]
    public bool RefreshIaipData { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Enter a facility ID.")]
    [RegularExpression(FacilityId.FacilityIdPattern, ErrorMessage = FacilityIdFormatError)]
    public string? FindId { get; set; }

    private const string FacilityIdFormatError = "The Facility ID entered is not valid.";

    public async Task<IActionResult> OnGetAsync([FromQuery] bool refresh = false)
    {
        if (refresh)
        {
            RefreshIaipData = true;
            return RedirectToPage();
        }

        Facilities = await service.GetListAsync(RefreshIaipData);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Facilities = await service.GetListAsync(RefreshIaipData);
            return Page();
        }

        if (FindId == null || !FacilityId.IsValidFormat(FindId))
            ModelState.AddModelError(nameof(FindId), FacilityIdFormatError);
        else if (!await service.ExistsAsync((FacilityId)FindId))
            ModelState.AddModelError(nameof(FindId),
                "A Facility with that ID does not exist or has not been approved in the IAIP.");

        if (ModelState.IsValid)
            return RedirectToPage("Details", routeValues: new { facilityId = FindId });

        Facilities = await service.GetListAsync(RefreshIaipData);
        return Page();
    }
}
