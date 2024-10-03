using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Compliance.FCE;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class AddModel(
    IFceService fceService,
    IFacilityService facilityService,
    IStaffService staffService,
    IValidator<FceCreateDto> validator) : PageModel
{
    [BindProperty]
    public FceCreateDto Item { get; set; } = default!;

    public SelectList StaffSelectList { get; private set; } = default!;
    public static SelectList YearSelectList { get; } = new(Fce.ValidFceYears);
    public IaipDataService.Facilities.Facility? Facility { get; private set; }

    public async Task<IActionResult> OnGetAsync(string? facilityId)
    {
        if (facilityId is null) return NotFound("Facility ID not found.");
        Facility = await facilityService.FindAsync((FacilityId)facilityId);

        // FUTURE: Add a facility search feature to the page?
        if (Facility is null) return NotFound("Facility ID not found.");

        await PopulateSelectListsAsync();
        var currentUserId = (await staffService.GetCurrentUserAsync()).Id;
        Item = new FceCreateDto((FacilityId)facilityId!, currentUserId);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (Item.FacilityId is null) return NotFound("Facility ID not found.");
        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            Facility = await facilityService.FindAsync((FacilityId)Item.FacilityId, token);
            if (Facility is null) return BadRequest("Facility ID not found.");

            await PopulateSelectListsAsync();
            return Page();
        }

        var createResult = await fceService.CreateAsync(Item, token);

        const string message = "FCE successfully created.";
        if (createResult.HasAppNotificationFailure)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning, message,
                createResult.AppNotificationResult!.FailureMessage);
        }
        else
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, message);
        }

        return RedirectToPage("Details", new { createResult.Id });
    }

    private async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();
}
