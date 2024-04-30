using GaEpd.AppLibrary.Pagination;
using MyApp.AppServices.WorkEntries.QueryDto;

namespace MyApp.WebApp.Models;

public record SearchResultsDisplay(
    IBasicSearchDisplay Spec,
    IPaginatedResult<WorkEntrySearchResultDto> SearchResults,
    PaginationNavModel Pagination,
    bool IsPublic)
{
    public string SortByName => Spec.Sort.ToString();
}
