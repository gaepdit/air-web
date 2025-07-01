using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.WebApp.Models;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Compliance.FCE;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class AddModel(
    IFceService fceService,
    IFacilityService facilityService,
    IStaffService staffService,
    IValidator<FceCreateDto> validator)
    : PageModel, ISubmitCancelButtons
{
    [FromRoute]
    public string? FacilityId { get; set; }

    [BindProperty]
    public FceCreateDto Item { get; set; } = null!;

    public SelectList StaffSelectList { get; private set; } = null!;
    public static SelectList YearSelectList { get; } = new(Fce.ValidFceYears);
    public IaipDataService.Facilities.Facility? Facility { get; private set; }
    private const string FacilityIdNotFound = "Facility ID not found.";

    // Form buttons
    public string SubmitText => "Add New FCE";
    public string CancelRoute => "/Facility/Details";
    public string RouteId => FacilityId ?? string.Empty;

    public async Task<IActionResult> OnGetAsync()
    {
        if (FacilityId is null) return NotFound(FacilityIdNotFound);
        Facility = await facilityService.FindFacilitySummaryAsync((FacilityId)FacilityId);

        // FUTURE: Add a facility search feature to the page?
        if (Facility is null) return NotFound(FacilityIdNotFound);

        await PopulateSelectListsAsync();
        var currentUserId = (await staffService.GetCurrentUserAsync()).Id;
        Item = new FceCreateDto((FacilityId)FacilityId, currentUserId);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (Item.FacilityId is null) return NotFound(FacilityIdNotFound);
        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            Facility = await facilityService.FindFacilitySummaryAsync((FacilityId)Item.FacilityId);
            if (Facility is null) return BadRequest(FacilityIdNotFound);

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
