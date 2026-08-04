using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Inspections;
using AirWeb.AppServices.Compliance.Enforcement;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using IaipDataService.Facilities;
using Microsoft.Identity.Web;

namespace AirWeb.WebApp.Pages.Compliance.Work.Add;

public class InspectionAddModel(
    IFacilityService facilityService,
    IComplianceWorkService complianceService,
    ICaseFileService caseFileService,
    IStaffService staffService,
    IValidator<InspectionCreateDto> validator)
    : AddBase(facilityService, complianceService, caseFileService, staffService)
{
    [BindProperty]
    public InspectionCreateDto Item { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(bool isRmp = false, CancellationToken token = default)
    {
        WorkType = isRmp ? ComplianceWorkType.RmpInspection : ComplianceWorkType.Inspection;

        Item = new InspectionCreateDto
        {
            ResponsibleStaffId = User.GetNameIdentifierId(),
            IsRmpInspection = isRmp,
            InspectionReason = isRmp ? InspectionReason.PlannedAnnounced : InspectionReason.PlannedUnannounced,
            FacilityOperating = isRmp,
            CaseFileId = CaseFileId,
        };

        return await DoGetAsync(token);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        WorkType = Item.IsRmpInspection ? ComplianceWorkType.RmpInspection : ComplianceWorkType.Inspection;
        return await DoPostAsync(Item, validator, token);
    }
}
