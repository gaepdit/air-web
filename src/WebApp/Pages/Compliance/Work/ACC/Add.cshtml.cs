using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.Accs;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.WebApp.Pages.Compliance.Work.WorkEntryBase;
using FluentValidation;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Compliance.Work.ACC;

public class AddModel(
    IWorkEntryService entryService,
    IFacilityService facilityService,
    IStaffService staffService,
    IValidator<AccCreateDto> validator)
    : AddBase(facilityService, staffService)
{
    private readonly IStaffService _staffService = staffService;

    [BindProperty]
    public AccCreateDto Item { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        EntryType = WorkEntryType.AnnualComplianceCertification;

        Item = new AccCreateDto
        {
            FacilityId = FacilityId,
            ResponsibleStaffId = (await _staffService.GetCurrentUserAsync()).Id,
        };

        return await DoGetAsync();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        EntryType = WorkEntryType.AnnualComplianceCertification;
        return await DoPostAsync(Item, entryService, validator, token);
    }
}
