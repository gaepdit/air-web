using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Query;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.AppRoles;
using AirWeb.WebApp.Models;
using AutoMapper;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Compliance.Work.Edit;

[Authorize(Policy = nameof(CompliancePolicies.ComplianceStaff))]
public abstract class EditBase(IComplianceWorkService service, IStaffService staffService, IMapper mapper)
    : PageModel, ISubmitCancelButtons
{
    protected readonly IMapper Mapper = mapper;

    [FromRoute]
    public int Id { get; set; }

    public IComplianceWorkSummaryDto ItemView { get; protected set; } = null!;
    public SelectList StaffSelectList { get; private set; } = null!;

    // Form buttons
    public string SubmitText => "Save Changes";
    public string CancelRoute => "../Details";
    public string RouteId => Id.ToString();

    protected async Task<IActionResult> DoGetAsync(CancellationToken token)
    {
        if (Id == 0) return RedirectToPage("../Index");

        var itemView = await service.FindAsync(Id, false, token);
        if (itemView is null) return NotFound();
        if (!User.CanEdit(itemView)) return Forbid();

        ItemView = itemView;

        await PopulateSelectListsAsync();
        return Page();
    }

    protected async Task<IActionResult> DoPostAsync<TDto>(
        TDto item, IValidator<TDto> validator, CancellationToken token)
        where TDto : IComplianceWorkCommandDto
    {
        var itemView = await service.FindSummaryAsync(Id, token);
        if (itemView is null || !User.CanEdit(itemView)) return BadRequest();
        await validator.ApplyValidationAsync(item, ModelState);

        if (!ModelState.IsValid)
        {
            ItemView = itemView;
            await PopulateSelectListsAsync();
            return Page();
        }

        var result = await service.UpdateAsync(Id, item, token);
        var workType = await service.GetComplianceWorkTypeAsync(Id, token);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success,
            $"{workType!.Value.GetDisplayName()} successfully updated.");
        if (result.HasWarning) TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, result.WarningMessage);
        return RedirectToPage("../Details", new { Id });
    }

    // FUTURE: Allow for editing a Compliance Work entry previously reviewed by a currently inactive user.
    protected virtual async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetUsersInRoleAsync(ComplianceRole.ComplianceStaffRole)).ToSelectList();
}
