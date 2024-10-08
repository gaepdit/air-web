﻿using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.NamedEntities.Offices;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Constants;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Pages.Compliance.FCE;

[Authorize(Policy = nameof(Policies.Staff))]
public class FceIndexModel(
    IComplianceSearchService searchService,
    IStaffService staff,
    IOfficeService offices,
    IAuthorizationService authorization)
    : PageModel
{
    public FceSearchDto Spec { get; set; } = default!;
    public bool ShowResults { get; private set; }
    public bool UserCanViewDeletedRecords { get; private set; }
    public IPaginatedResult<FceSearchResultDto> SearchResults { get; private set; } = default!;
    public PaginatedResultsDisplay ResultsDisplay => new(Spec, SearchResults);

    // Select lists
    public SelectList StaffSelectList { get; private set; } = default!;
    public SelectList OfficesSelectList { get; set; } = default!;

    private static int _finalYear = DateTime.Today.Month > 9 ? DateTime.Now.Year + 1 : DateTime.Now.Year;
    private static int _years = _finalYear - Fce.EarliestFceYear + 1;
    public static SelectList YearSelectList => new(Enumerable.Range(Fce.EarliestFceYear, _years).Reverse());

    public async Task OnGetAsync()
    {
        UserCanViewDeletedRecords = await authorization.Succeeded(User, Policies.ComplianceManager);
        await PopulateSelectListsAsync();
    }

    public async Task OnGetSearchAsync(FceSearchDto spec, [FromQuery] int p = 1,
        CancellationToken token = default)
    {
        Spec = spec.TrimAll();
        UserCanViewDeletedRecords = await authorization.Succeeded(User, Policies.ComplianceManager);
        if (!UserCanViewDeletedRecords) Spec = Spec with { DeleteStatus = null };

        var paging = new PaginatedRequest(pageNumber: p, GlobalConstants.PageSize, sorting: Spec.Sort.GetDescription());
        SearchResults = await searchService.SearchFcesAsync(Spec, paging, token);

        ShowResults = true;
        await PopulateSelectListsAsync();
    }

    private async Task PopulateSelectListsAsync()
    {
        StaffSelectList = (await staff.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
        OfficesSelectList = (await offices.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
    }
}
