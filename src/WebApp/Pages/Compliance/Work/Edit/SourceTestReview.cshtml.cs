using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.SourceTestReviews;
using AirWeb.AppServices.Staff;
using AutoMapper;
using FluentValidation;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace AirWeb.WebApp.Pages.Compliance.Work.Edit;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class SourceTestReviewEditModel(
    IComplianceWorkService service,
    ISourceTestService sourceTestService,
    IStaffService staffService,
    IMapper mapper,
    IValidator<SourceTestReviewUpdateDto> validator)
    : EditBase(service, staffService, mapper)
{
    [BindProperty]
    public SourceTestReviewUpdateDto Item { get; set; } = null!;

    public SourceTestSummary TestSummary { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(CancellationToken token)
    {
        var result = await DoGetAsync(token);
        if (result is not PageResult) return result;

        Item = Mapper.Map<SourceTestReviewUpdateDto>(ItemView);

        var testSummary = await sourceTestService.FindSummaryAsync(Item.ReferenceNumber);
        if (testSummary is null) return BadRequest();
        TestSummary = testSummary;

        return result;
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var result = await DoPostAsync(Item, validator, token);
        if (result is not PageResult) return result;

        var testSummary = await sourceTestService.FindSummaryAsync(Item.ReferenceNumber);
        if (testSummary is null || testSummary.ReferenceNumber != Item.ReferenceNumber) return BadRequest();
        TestSummary = testSummary;

        return result;
    }
}
