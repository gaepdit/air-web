using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.Staff;
using AirWeb.WebApp.Pages.Compliance.Work.WorkEntryBase;
using FluentValidation;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace AirWeb.WebApp.Pages.Compliance.Work.SourceTestReview;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class EditModel(
    IWorkEntryService entryService,
    ISourceTestService sourceTestService,
    IStaffService staffService,
    IAuthorizationService authorization,
    IValidator<SourceTestReviewUpdateDto> validator)
    : EditBase(entryService, staffService)
{
    private readonly IWorkEntryService _entryService = entryService;

    [BindProperty]
    public SourceTestReviewUpdateDto Item { get; set; } = default!;

    public SourceTestSummary TestSummary { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("../Index");

        var item = (SourceTestReviewUpdateDto?)await _entryService.FindForUpdateAsync(Id);
        if (item is null) return NotFound();
        if (!await UserCanEditAsync(item)) return Forbid();

        var testSummary = await sourceTestService.FindSummaryAsync(item.ReferenceNumber);
        if (testSummary is null) return BadRequest();

        Item = item;
        TestSummary = testSummary;

        return await DoGetAsync();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var original = (SourceTestReviewUpdateDto?)await _entryService.FindForUpdateAsync(Id, token);
        if (original is null || !await UserCanEditAsync(original)) return BadRequest();

        var testSummary = await sourceTestService.FindSummaryAsync(original.ReferenceNumber);
        if (testSummary is null) return BadRequest();

        TestSummary = testSummary;

        return await DoPostAsync(Item, validator, token);
    }

    private Task<bool> UserCanEditAsync(SourceTestReviewUpdateDto item) =>
        authorization.Succeeded(User, item, new SourceTestReviewUpdateRequirement());
}
