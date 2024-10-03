using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.Reports;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.WebApp.Pages.Compliance.Work.WorkEntryBase;
using FluentValidation;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Compliance.Work.Report;

public class AddModel(
    IWorkEntryService entryService,
    IFacilityService facilityService,
    IStaffService staffService,
    IValidator<ReportCreateDto> validator)
    : AddBase(facilityService, staffService)
{
    private readonly IStaffService _staffService = staffService;

    [BindProperty]
    public ReportCreateDto Item { get; set; } = default!;

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
