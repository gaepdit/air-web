using AirWeb.AppServices.Core.Search;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Models;

public record PaginatedResultsDisplay(
    IPaginatedResult SearchResults,
    IDictionary<string, string?> RouteValues,
    string SortByName = "default")
{
    public PaginatedResultsDisplay(ISearchDto spec, IPaginatedResult searchResults) :
        this(searchResults, spec.AsRouteValues(), spec.SortByName) { }

    public string SearchHandler { get; init; } = "Search";
    public string SearchFragment { get; init; } = "SearchResults";
}
