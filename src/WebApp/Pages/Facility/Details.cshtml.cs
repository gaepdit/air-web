﻿using AirWeb.AppServices.Compliance.Fces.Search;
using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.WebApp.Platform.Constants;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace AirWeb.WebApp.Pages.Facility;

[Authorize(Policy = nameof(Policies.Staff))]
public class DetailsModel(
    IFacilityService facilityService,
    IFceSearchService fceSearchService,
    IWorkEntrySearchService entrySearchService,
    ISourceTestService sourceTestService,
    IAuthorizationService authorization) : PageModel
{
    // Facility
    [FromRoute]
    public string? Id { get; set; }

    public IaipDataService.Facilities.Facility? Facility { get; private set; }

    // Data tables
    public IList<WorkEntrySearchResultDto> ComplianceWork { get; private set; } = [];
    public int ComplianceWorkCount { get; private set; }
    public IList<FceSearchResultDto> Fces { get; private set; } = [];
    public int FceCount { get; private set; }
    public IList<SourceTestSummary> SourceTests { get; private set; } = [];
    public int SourceTestCount { get; private set; }

    [TempData]
    public bool RefreshIaipData { get; set; }

    // Permissions
    public bool IsComplianceStaff { get; private set; }

    public async Task<IActionResult> OnGetAsync([FromQuery] bool refresh = false, CancellationToken token = default)
    {
        if (refresh)
        {
            RefreshIaipData = true;
            return RedirectToPage();
        }

        if (Id is null) return NotFound("Facility ID not found.");
        Facility = await facilityService.FindFacilityDetailsAsync((FacilityId)Id, RefreshIaipData);
        if (Facility is null) return NotFound("Facility ID not found.");

        // Source Test service can be run in parallel with the search service.
        var sourceTestsForFacilityTask =
            sourceTestService.GetSourceTestsForFacilityAsync((FacilityId)Id, RefreshIaipData);

        // Search service cannot be run in parallel with itself when using Entity Framework.
        var searchWorkEntries = await entrySearchService.SearchAsync(
            new WorkEntrySearchDto { Sort = SortBy.EventDateDesc, PartialFacilityId = Id },
            new PaginatedRequest(1, GlobalConstants.SummaryTableSize),
            loadFacilities: false, token: token);

        var searchFces = await fceSearchService.SearchAsync(
            new FceSearchDto { Sort = SortBy.EventDateDesc, PartialFacilityId = Id },
            new PaginatedRequest(1, GlobalConstants.SummaryTableSize),
            loadFacilities: false, token: token);

        ComplianceWork = searchWorkEntries.Items.ToList();
        ComplianceWorkCount = searchWorkEntries.TotalCount;

        Fces = searchFces.Items.ToList();
        FceCount = searchFces.TotalCount;

        var sourceTestsForFacility = await sourceTestsForFacilityTask;
        SourceTests = sourceTestsForFacility.Take(GlobalConstants.SummaryTableSize).ToList();
        SourceTestCount = sourceTestsForFacility.Count;

        IsComplianceStaff = await authorization.Succeeded(User, Policies.ComplianceStaff);
        return Page();
    }
}
