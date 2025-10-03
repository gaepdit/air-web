using AirWeb.AppServices.Compliance.ComplianceWork;
using AirWeb.AppServices.Compliance.ComplianceWork.Reports;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using FluentValidation;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Compliance.Work.Add;

public class ReportAddModel(
    IWorkEntryService entryService,
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
        EntryType = WorkEntryType.Report;

        Item = new ReportCreateDto
        {
            FacilityId = FacilityId,
            ResponsibleStaffId = (await _staffService.GetCurrentUserAsync()).Id,
        };

        return await DoGetAsync();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        EntryType = WorkEntryType.Report;
        return await DoPostAsync(Item, entryService, validator, token);
    }
}
