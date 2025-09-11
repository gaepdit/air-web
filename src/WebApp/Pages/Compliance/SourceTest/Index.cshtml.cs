﻿using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.SourceTests;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Defaults;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.SourceTests.Models;

namespace AirWeb.WebApp.Pages.Compliance.SourceTest;

[Authorize(Policy = nameof(Policies.Staff))]
public class SourceTestIndexModel(ISourceTestsService sourceTestService) : PageModel
{
    public IPaginatedResult<SourceTestSummary> SearchResults { get; private set; } = null!;
    public PaginatedResultsDisplay ResultsDisplay => new(SearchResults);
    public int PageNumber { get; set; }

    [TempData]
    public bool RefreshIaipData { get; set; }

    public async Task<IActionResult> OnGetAsync([FromQuery] int p = 1, [FromQuery] bool refresh = false,
        CancellationToken token = default)
    {
        if (refresh)
        {
            RefreshIaipData = true;
            return RedirectToPage(new { p });
        }

        PageNumber = p;
        var paging = new PaginatedRequest(pageNumber: p, GlobalConstants.PageSize, sorting: "default");
        SearchResults = await sourceTestService.GetOpenSourceTestsForComplianceAsync(userEmail: null, paging,
            RefreshIaipData);

        return Page();
    }

    public Task<IActionResult> OnGetSearchAsync([FromQuery] int p = 1, [FromQuery] bool refresh = false,
        CancellationToken token = default) =>
        OnGetAsync(p, refresh, token);
}
