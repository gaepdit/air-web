using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Reports;
using AirWeb.AppServices.Compliance.Enforcement;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using IaipDataService.Facilities;
using Microsoft.Identity.Web;

namespace AirWeb.WebApp.Pages.Compliance.Work.Add;

public class ReportAddModel(
    IFacilityService facilityService,
    IComplianceWorkService complianceService,
    ICaseFileService caseFileService,
    IStaffService staffService,
    IValidator<ReportCreateDto> validator)
    : AddBase(facilityService, complianceService, caseFileService, staffService)
{
    [BindProperty]
    public ReportCreateDto Item { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(CancellationToken token = default)
    {
        WorkType = ComplianceWorkType.Report;

        var yesterday = DateOnly.FromDateTime(DateTime.Today).AddDays(-1);
        Item = new ReportCreateDto
        {
            ResponsibleStaffId = User.GetNameIdentifierId(),
            ReportingPeriodStart = yesterday,
            ReportingPeriodEnd = yesterday,
            CaseFileId = CaseFileId,
        };

        return await DoGetAsync(token);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        WorkType = ComplianceWorkType.Report;
        return await DoPostAsync(Item, validator, token);
    }
}
