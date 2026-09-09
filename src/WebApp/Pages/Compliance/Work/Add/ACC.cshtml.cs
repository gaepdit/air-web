using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Accs;
using AirWeb.AppServices.Compliance.Enforcement;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using IaipDataService.Facilities;
using Microsoft.Identity.Web;

namespace AirWeb.WebApp.Pages.Compliance.Work.Add;

public class AccAddModel(
    IFacilityService facilityService,
    IComplianceWorkService complianceService,
    ICaseFileService caseFileService,
    IStaffService staffService,
    IValidator<AccCreateDto> validator)
    : AddBase(facilityService, complianceService, caseFileService, staffService)
{
    [BindProperty]
    public AccCreateDto Item { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(CancellationToken token = default)
    {
        WorkType = ComplianceWorkType.AnnualComplianceCertification;

        Item = new AccCreateDto
        {
            ResponsibleStaffId = User.GetNameIdentifierId(),
            CaseFileId = CaseFileId,
        };

        return await DoGetAsync(token);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        WorkType = ComplianceWorkType.AnnualComplianceCertification;
        return await DoPostAsync(Item, validator, token);
    }
}
