using AirWeb.AppServices.Compliance.ComplianceWork;
using AirWeb.AppServices.Compliance.ComplianceWork.PermitRevocations;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using FluentValidation;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Compliance.Work.Add;

public class PermitRevocationAddModel(
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
