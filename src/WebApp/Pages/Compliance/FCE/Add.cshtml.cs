using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;

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

    public FacilityViewDto? Facility { get; private set; }
    public SelectList StaffSelectList { get; private set; } = default!;
    public static SelectList YearSelectList { get; } = new(Fce.ValidFceYears);

    public async Task OnGetAsync(string? facilityId)
    {
        Facility = await facilityService.FindAsync(facilityId);

        // FUTURE: Add a facility search feature to the page?
        if (Facility is null) return;

        await PopulateSelectListsAsync();
        var currentUserId = (await staffService.GetCurrentUserAsync()).Id;
        Item = new FceCreateDto((FacilityId)facilityId!, currentUserId);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            Facility = await facilityService.FindAsync(Item.FacilityId, token);
            if (Facility is null) return BadRequest("Facility ID not found.");

            await PopulateSelectListsAsync();
            return Page();
        }

        var createResult = await fceService.CreateAsync(Item, token);

        if (createResult.HasAppNotificationFailure)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning, "FCE successfully created.",
                createResult.AppNotificationResult!.FailureMessage);
        }
        else
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "FCE successfully created.");
        }

        return RedirectToPage("Details", new { createResult.Id });
    }

    private async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();
}
