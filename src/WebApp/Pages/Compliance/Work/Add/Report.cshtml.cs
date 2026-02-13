using AirWeb.AppServices.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.Reports;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using FluentValidation;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Compliance.Work.Add;

public class ReportAddModel(
    IComplianceWorkService service,
    IFacilityService facilityService,
    IStaffService staffService,
    IValidator<ReportCreateDto> validator)
    : AddBase(facilityService, staffService)
{
    private readonly IStaffService _staffService = staffService;

    [BindProperty]
    public ReportCreateDto Item { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        WorkType = ComplianceWorkType.Report;

        Item = new ReportCreateDto
        {
            FacilityId = FacilityId,
            ResponsibleStaffId = (await _staffService.GetCurrentUserAsync()).Id,
        };

        return await DoGetAsync();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        WorkType = ComplianceWorkType.Report;
        return await DoPostAsync(Item, service, validator, token);
    }
}
