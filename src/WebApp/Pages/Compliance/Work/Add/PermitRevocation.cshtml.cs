using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.PermitRevocations;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using IaipDataService.Facilities;
using Microsoft.Identity.Web;

namespace AirWeb.WebApp.Pages.Compliance.Work.Add;

public class PermitRevocationAddModel(
    IFacilityService facilityService,
    IComplianceWorkService complianceService,
    IStaffService staffService,
    IValidator<PermitRevocationCreateDto> validator)
    : AddBase(facilityService, complianceService, null, staffService)
{
    [BindProperty]
    public PermitRevocationCreateDto Item { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(CancellationToken token = default)
    {
        WorkType = ComplianceWorkType.PermitRevocation;
        Item = new PermitRevocationCreateDto { ResponsibleStaffId = User.GetNameIdentifierId() };
        return await DoGetAsync(token);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        WorkType = ComplianceWorkType.PermitRevocation;
        return await DoPostAsync(Item, validator, token);
    }
}
