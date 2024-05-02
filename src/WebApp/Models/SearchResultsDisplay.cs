﻿using GaEpd.AppLibrary.Pagination;
using AirWeb.AppServices.WorkEntries.QueryDto;

namespace AirWeb.WebApp.Models;

public record SearchResultsDisplay(
    IBasicSearchDisplay Spec,
    IPaginatedResult<WorkEntrySearchResultDto> SearchResults,
    PaginationNavModel Pagination,
    bool IsPublic)
{
    public string SortByName => Spec.Sort.ToString();
}
