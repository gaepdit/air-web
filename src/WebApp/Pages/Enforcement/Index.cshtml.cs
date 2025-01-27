using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.CaseFiles;
using AirWeb.AppServices.Permissions;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(Policies.Staff))]
public class IndexModel(IEnforcementService service) : PageModel
{
    public IReadOnlyCollection<CaseFileSummaryDto> CaseFiles { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync([FromQuery] bool refresh = false, CancellationToken token = default)
    {
        CaseFiles = await service.GetListAsync(token);
        return Page();
    }
}
