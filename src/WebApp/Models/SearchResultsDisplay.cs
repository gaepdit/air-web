using AirWeb.AppServices.Compliance.Search;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Models;

public record SearchResultsDisplay(
    IStandardSearch Spec,
    IPaginatedResult<WorkEntrySearchResultDto> SearchResults)
{
    public string SortByName => Spec.Sort.ToString();
    public IDictionary<string, string?> RouteValues => Spec.AsRouteValues();
}
