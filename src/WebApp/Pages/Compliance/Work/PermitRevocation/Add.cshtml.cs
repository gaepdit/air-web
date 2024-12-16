using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.WebApp.Pages.Compliance.Work.WorkEntryBase;
using FluentValidation;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Compliance.Work.PermitRevocation;

public class AddModel(
    IWorkEntryService entryService,
    IFacilityService facilityService,
    IStaffService staffService,
    IValidator<PermitRevocationCreateDto> validator)
    : AddBase(facilityService, staffService)
{
    private readonly IStaffService _staffService = staffService;

    [BindProperty]
    public PermitRevocationCreateDto Item { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        EntryType = WorkEntryType.PermitRevocation;

        Item = new PermitRevocationCreateDto
        {
            FacilityId = FacilityId,
            ResponsibleStaffId = (await _staffService.GetCurrentUserAsync()).Id,
        };

        return await DoGetAsync();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        EntryType = WorkEntryType.PermitRevocation;
        return await DoPostAsync(Item, entryService, validator, token);
    }
}
