using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;
using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Compliance.Work.WorkEntryBase;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public abstract class AddBase(IFacilityService facilityService, IStaffService staffService) : PageModel
{
    [FromRoute]
    public string? FacilityId { get; set; }

    public WorkEntryType EntryType { get; protected set; }
    public FacilityViewDto? Facility { get; protected set; }
    public SelectList StaffSelectList { get; private set; } = default!;

    protected async Task<IActionResult> DoGetAsync()
    {
        Facility = await facilityService.FindAsync(FacilityId);
        if (Facility is null) return NotFound("Facility ID not found.");

        await PopulateSelectListsAsync();
        return Page();
    }

    protected async Task<IActionResult> DoPostAsync<TDto>(
        TDto item, IWorkEntryService entryService,
        IValidator<TDto> validator, CancellationToken token)
        where TDto : IWorkEntryCreateDto
    {
        if (FacilityId != item.FacilityId) return BadRequest();
        await validator.ApplyValidationAsync(item, ModelState);

        if (!ModelState.IsValid)
        {
            Facility = await facilityService.FindAsync(item.FacilityId, token);
            if (Facility is null) return BadRequest();

            await PopulateSelectListsAsync();
            return Page();
        }

        var createResult = await entryService.CreateAsync(item, token);

        var message = $"{EntryType.GetDescription()} successfully created.";
        if (createResult.HasAppNotificationFailure)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning, message,
                createResult.AppNotificationResult!.FailureMessage);
        }
        else
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, message);
        }

        return RedirectToPage("../Details", new { createResult.Id });
    }

    protected virtual async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();
}
