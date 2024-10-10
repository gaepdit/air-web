using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Pages.Compliance.Work.WorkEntryBase;
using AirWeb.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace AirWeb.WebApp.Pages.Compliance.Work.SourceTestReview;



// Move this to section within TestReport



[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class AddModel(
    ISourceTestService sourceTestService,
    IWorkEntryService entryService,
    IStaffService staffService,
    IValidator<SourceTestReviewCreateDto> validator) : PageModel
{
    [FromRoute]
    public int ReferenceNumber { get; set; }

    [BindProperty]
    public SourceTestReviewCreateDto Item { get; set; } = default!;

    public SourceTestSummary? TestSummary { get; private set; }
    public SelectList StaffSelectList { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (ReferenceNumber > 0) TestSummary = await sourceTestService.FindSummaryAsync(ReferenceNumber);
        if (TestSummary is null) return NotFound("Facility ID not found.");

        Item = new SourceTestReviewCreateDto
        {
            ReferenceNumber = ReferenceNumber,
            FacilityId = TestSummary.Facility?.FacilityId,
            ResponsibleStaffId = (await staffService.GetCurrentUserAsync()).Id,
        };

        await PopulateSelectListsAsync();
        return Page();
    }


    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (ReferenceNumber != Item.ReferenceNumber || Item.FacilityId == null) return BadRequest();
        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            TestSummary = await sourceTestService.FindSummaryAsync(ReferenceNumber);
            if (TestSummary is null) return BadRequest();

            await PopulateSelectListsAsync();
            return Page();
        }

        var createResult = await entryService.CreateAsync(Item, token);

        const string message = "Source Test Review successfully created.";
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

    private async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();
}
