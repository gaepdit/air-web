using AirWeb.AppServices.CommonSearch;
using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Staff.Dto;
using ComplianceSortBy = AirWeb.AppServices.Compliance.Search.SortBy;
using UserSortBy = AirWeb.AppServices.Staff.Dto.SortBy;

namespace AirWeb.WebApp.Models;

public record SearchResultsColumnDisplay
{
    public SearchResultsColumnDisplay(string heading, ComplianceSortBy ascender, ComplianceSortBy descender,
        ISearchDto spec)
    {
        Heading = heading;
        Up = ascender.ToString();
        Down = descender.ToString();
        SortByName = spec.Sort.ToString();
        RouteValues = spec.AsRouteValues();
    }

    public SearchResultsColumnDisplay(string heading, UserSortBy ascender, UserSortBy descender, StaffSearchDto spec)
    {
        Heading = heading;
        Up = ascender.ToString();
        Down = descender.ToString();
        SortByName = spec.Sort.ToString();
        RouteValues = spec.AsRouteValues();
    }

    public string Heading { get; init; }
    public string SortByName { get; }
    public string Up { get; }
    public string Down { get; }
    public IDictionary<string, string?> RouteValues { get; }
}
