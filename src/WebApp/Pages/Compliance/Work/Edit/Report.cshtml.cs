using AirWeb.AppServices.Compliance.ComplianceWork;
using AirWeb.AppServices.Compliance.ComplianceWork.Reports;
using AirWeb.AppServices.Staff;
using AutoMapper;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Compliance.Work.Edit;

public class ReportEditModel(
    IWorkEntryService entryService,
    IStaffService staffService,
    IMapper mapper,
    IValidator<ReportUpdateDto> validator)
    : EditBase(entryService, staffService, mapper)
{
    [BindProperty]
    public ReportUpdateDto Item { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        var result = await DoGetAsync(token);
        if (result is not PageResult) return result;
        Item = Mapper.Map<ReportUpdateDto>(ItemView);
        return result;
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token) =>
        await DoPostAsync(Item, validator, token);
}
