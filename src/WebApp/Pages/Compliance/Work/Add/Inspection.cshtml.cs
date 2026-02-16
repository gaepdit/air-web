using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Inspections;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using FluentValidation;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Compliance.Work.Add;

public class InspectionAddModel(
    IComplianceWorkService service,
    IFacilityService facilityService,
    IStaffService staffService,
    IValidator<InspectionCreateDto> validator)
    : AddBase(facilityService, staffService)
{
    private readonly IStaffService _staffService = staffService;

    [BindProperty]
    public InspectionCreateDto Item { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(bool isRmp = false)
    {
        WorkType = isRmp ? ComplianceWorkType.RmpInspection : ComplianceWorkType.Inspection;

        Item = new InspectionCreateDto
        {
            FacilityId = FacilityId,
            ResponsibleStaffId = (await _staffService.GetCurrentUserAsync()).Id,
            IsRmpInspection = isRmp,
        };

        return await DoGetAsync();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        WorkType = Item.IsRmpInspection ? ComplianceWorkType.RmpInspection : ComplianceWorkType.Inspection;
        return await DoPostAsync(Item, service, validator, token);
    }
}
