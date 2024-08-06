using AirWeb.AppServices.Compliance.Search;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Models;

public record SearchResultsDisplay(
    IBasicSearchDisplay Spec,
    IPaginatedResult<WorkEntrySearchResultDto> SearchResults,
    PaginationNavModel Pagination,
    bool IsPublic)
{
    public string SortByName => Spec.Sort.ToString();
}
