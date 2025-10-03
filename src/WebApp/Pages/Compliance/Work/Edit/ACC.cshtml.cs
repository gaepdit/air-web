using AirWeb.AppServices.Compliance.ComplianceWork;
using AirWeb.AppServices.Compliance.ComplianceWork.Accs;
using AirWeb.AppServices.Staff;
using AutoMapper;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Compliance.Work.Edit;

public class AccEditModel(
    IWorkEntryService entryService,
    IStaffService staffService,
    IMapper mapper,
    IValidator<AccUpdateDto> validator)
    : EditBase(entryService, staffService, mapper)
{
    [BindProperty]
    public AccUpdateDto Item { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        var result = await DoGetAsync(token);
        if (result is not PageResult) return result;
        Item = Mapper.Map<AccUpdateDto>(ItemView);
        return result;
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token) =>
        await DoPostAsync(Item, validator, token);
}
