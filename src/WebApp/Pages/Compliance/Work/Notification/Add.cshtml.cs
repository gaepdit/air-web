using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.Notifications;
using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.NamedEntities.NotificationTypes;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Pages.Compliance.Work.WorkEntryBase;
using AirWeb.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Compliance.Work.Notification;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class AddModel(
    IWorkEntryService entryService,
    IFacilityService facilityService,
    INotificationTypeService notificationTypeService,
    IStaffService staffService,
    IValidator<NotificationCreateDto> validator)
    : AddBase(staffService)
{
    private readonly IStaffService _staffService = staffService;

    [BindProperty]
    public NotificationCreateDto Item { get; set; } = default!;

    public SelectList NotificationTypeSelectList { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(string? facilityId)
    {
        EntryType = WorkEntryType.Notification;

        Facility = await facilityService.FindAsync(facilityId);
        if (Facility is null) return NotFound("Facility ID not found.");

        Item = new NotificationCreateDto
        {
            FacilityId = facilityId,
            ResponsibleStaffId = (await _staffService.GetCurrentUserAsync()).Id,
        };

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        EntryType = WorkEntryType.Notification;

        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            Facility = await facilityService.FindAsync(Item.FacilityId, token);
            if (Facility is null) return BadRequest("Facility ID not found.");

            await PopulateSelectListsAsync();
            return Page();
        }

        var createResult = await entryService.CreateAsync(Item, token);

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

    protected async Task PopulateSelectListsAsync()
    {
        await PopulateStaffSelectListsAsync();
        NotificationTypeSelectList = (await notificationTypeService.GetAsListItemsAsync()).ToSelectList();
    }
}
