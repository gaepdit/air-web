using AirWeb.AppServices.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.Inspections;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AutoMapper;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Compliance.Work.Edit;

public class InspectionEditModel(
    IComplianceWorkService service,
    IStaffService staffService,
    IMapper mapper,
    IValidator<InspectionUpdateDto> validator)
    : EditBase(service, staffService, mapper)
{
    [BindProperty]
    public InspectionUpdateDto Item { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        var result = await DoGetAsync(token);
        if (result is not PageResult) return result;
        Item = Mapper.Map<InspectionUpdateDto>(ItemView);
        return result;
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token) =>
        await DoPostAsync(Item, validator, token);
}
