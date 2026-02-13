using AirWeb.AppServices.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.PermitRevocations;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using FluentValidation;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Compliance.Work.Add;

public class PermitRevocationAddModel(
    IComplianceWorkService service,
    IFacilityService facilityService,
    IStaffService staffService,
    IValidator<PermitRevocationCreateDto> validator)
    : AddBase(facilityService, staffService)
{
    private readonly IStaffService _staffService = staffService;

    [BindProperty]
    public PermitRevocationCreateDto Item { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        WorkType = ComplianceWorkType.PermitRevocation;

        Item = new PermitRevocationCreateDto
        {
            FacilityId = FacilityId,
            ResponsibleStaffId = (await _staffService.GetCurrentUserAsync()).Id,
        };

        return await DoGetAsync();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        WorkType = ComplianceWorkType.PermitRevocation;
        return await DoPostAsync(Item, service, validator, token);
    }
}
