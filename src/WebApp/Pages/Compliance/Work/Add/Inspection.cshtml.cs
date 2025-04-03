using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.Inspections;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using FluentValidation;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Compliance.Work.Add;

public class InspectionAddModel(
    IWorkEntryService entryService,
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
