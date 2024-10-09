using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace AirWeb.WebApp.Pages.Compliance.TestReport;

[Authorize(Policy = nameof(Policies.Staff))]
public class IndexModel(
    ISourceTestService testService,
    IWorkEntryService entryService,
    IAuthorizationService authorization) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    public SourceTestSummary? Item { get; private set; }
    public SourceTestReviewViewDto? ComplianceReview { get; private set; }
    public bool IsComplianceStaff { get; private set; }
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        if (Id == 0) return RedirectToPage("Index");
        Item = await testService.FindSummaryAsync(Id);
        if (Item is null) return NotFound();

        ComplianceReview = await entryService.FindSourceTestReviewAsync(Id, token);
        await SetPermissionsAsync();
        return Page();
    }

    private async Task SetPermissionsAsync()
    {
        IsComplianceStaff = await authorization.Succeeded(User, Policies.ComplianceStaff);
        UserCan = await authorization.SetPermissions(ComplianceWorkOperation.AllOperations, User, ComplianceReview);
    }
}
