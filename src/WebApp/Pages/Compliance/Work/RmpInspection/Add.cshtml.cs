using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.Inspections;
using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.WebApp.Pages.Compliance.Work.WorkEntryBase;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Compliance.Work.RmpInspection;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class AddModel(
    IWorkEntryService entryService,
    IFacilityService facilityService,
    IStaffService staffService,
    IValidator<InspectionCreateDto> validator)
    : AddBase(facilityService, staffService)
{
    private readonly IStaffService _staffService = staffService;

    [BindProperty]
    public InspectionCreateDto Item { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string? facilityId)
    {
        EntryType = WorkEntryType.RmpInspection;

        Item = new InspectionCreateDto
        {
            FacilityId = facilityId,
            ResponsibleStaffId = (await _staffService.GetCurrentUserAsync()).Id,
            IsRmpInspection = false,
        };

        return await DoGetAsync(facilityId);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        EntryType = WorkEntryType.RmpInspection;
        return await DoPostAsync(Item, entryService, validator, token);
    }
}
