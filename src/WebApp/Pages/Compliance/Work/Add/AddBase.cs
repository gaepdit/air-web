using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.Identity;
using AirWeb.WebApp.Models;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Compliance.Work.Add;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public abstract class AddBase(IFacilityService facilityService, IStaffService staffService)
    : PageModel, ISubmitCancelButtons
{
    [FromRoute]
    public string? FacilityId { get; set; }

    public ComplianceWorkType WorkType { get; protected set; }
    public IaipDataService.Facilities.Facility? Facility { get; protected set; }
    public SelectList StaffSelectList { get; private set; } = null!;

    // Form buttons
    public string SubmitText => $"Add {WorkType.GetDisplayName()}";
    public string CancelRoute => "/Facility/Details";
    public string RouteId => FacilityId ?? string.Empty;

    protected async Task<IActionResult> DoGetAsync()
    {
        if (FacilityId is null) return NotFound("Facility ID not found.");
        Facility = await facilityService.FindFacilityDetailsAsync((FacilityId)FacilityId);
        if (Facility is null) return NotFound("Facility ID not found.");

        await PopulateSelectListsAsync();
        return Page();
    }

    protected async Task<IActionResult> DoPostAsync<TDto>(
        TDto item, IComplianceWorkService service,
        IValidator<TDto> validator, CancellationToken token)
        where TDto : IComplianceWorkCreateDto
    {
        if (item.FacilityId == null || FacilityId != item.FacilityId) return BadRequest();
        await validator.ApplyValidationAsync(item, ModelState);

        if (!ModelState.IsValid)
        {
            Facility = await facilityService.FindFacilitySummaryAsync((FacilityId)item.FacilityId);
            if (Facility is null) return BadRequest();

            await PopulateSelectListsAsync();
            return Page();
        }

        var result = await service.CreateAsync(item, token);

        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success,
            $"{WorkType.GetDisplayName()} successfully created.");
        if (result.HasWarning) TempData.AddDisplayMessage(DisplayMessage.AlertContext.Warning, result.WarningMessage);
        return RedirectToPage("../Details", new { result.Id });
    }

    protected virtual async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetUsersInRoleAsync(AppRole.ComplianceStaffRole)).ToSelectList();
}
