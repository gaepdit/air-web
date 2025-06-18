using AirWeb.AppServices.CommonSearch;
using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Staff.Dto;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Models;

public record PaginatedResultsDisplay
{
    public PaginatedResultsDisplay(ISearchDto spec, IPaginatedResult searchResults)
    {
        SortByName = spec.Sort.ToString();
        SearchResults = searchResults;
        RouteValues = spec.AsRouteValues();
    }

    public PaginatedResultsDisplay(StaffSearchDto spec, IPaginatedResult searchResults)
    {
        SortByName = spec.Sort.ToString();
        SearchResults = searchResults;
        RouteValues = spec.AsRouteValues();
    }

    public string SortByName { get; }
    public IDictionary<string, string?> RouteValues { get; }
    public IPaginatedResult SearchResults { get; init; }
}
