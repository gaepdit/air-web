using AirWeb.AppServices.Core.AuthorizationServices;
using IaipDataService.Facilities;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.WebApp.Pages.Facility;

[Authorize(Policy = nameof(Policies.Staff))]
public class IndexModel(IFacilityService service) : PageModel
{
    public IReadOnlyCollection<FacilitySummary> Facilities { get; private set; } = null!;

    [TempData]
    public bool RefreshIaipData { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Enter a facility ID.")]
    [RegularExpression(FacilityId.FacilityIdEnclosedPattern, ErrorMessage = FacilityId.FacilityIdFormatError)]
    public string? FindId { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken token = default)
    {
        Facilities = await service.GetAllAsync(RefreshIaipData, token: token);
        return Page();
    }

    public IActionResult OnPostRefreshIaipAsync()
    {
        RefreshIaipData = true;
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (!ModelState.IsValid)
        {
            Facilities = await service.GetAllAsync(RefreshIaipData, token: token);
            return Page();
        }

        if (FindId == null || !FacilityId.IsValidFormat(FindId))
            ModelState.AddModelError(nameof(FindId), FacilityId.FacilityIdFormatError);
        else if (!await service.ExistsAsync((FacilityId)FindId))
            ModelState.AddModelError(nameof(FindId), FacilityId.FacilityNotExistsError);

        if (ModelState.IsValid)
            return RedirectToPage("Details", routeValues: new { id = FindId });

        Facilities = await service.GetAllAsync(RefreshIaipData, token: token);
        return Page();
    }
}
