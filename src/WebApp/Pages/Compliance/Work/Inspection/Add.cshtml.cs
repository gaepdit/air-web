using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.Inspections;
using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.WebApp.Pages.Compliance.Work.WorkEntryBase;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Compliance.Work.Inspection;

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

    public async Task<IActionResult> OnGetAsync(bool isRmp = false)
    {
        EntryType = isRmp ? WorkEntryType.RmpInspection : WorkEntryType.Inspection;

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
        EntryType = Item.IsRmpInspection ? WorkEntryType.RmpInspection : WorkEntryType.Inspection;
        return await DoPostAsync(Item, entryService, validator, token);
    }
}
