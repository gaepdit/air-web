using AirWeb.AppServices.CommonSearch;

namespace AirWeb.WebApp.Models;

public record SearchResultsColumnDisplay
{
    public SearchResultsColumnDisplay(string heading, string ascender, string descender,
        ISearchDto spec)
    {
        Heading = heading;
        Up = ascender;
        Down = descender;
        SortByName = spec.SortByName;
        RouteValues = spec.AsRouteValues();
    }

    public string Heading { get; init; }
    public string SortByName { get; }
    public string Up { get; }
    public string Down { get; }
    public IDictionary<string, string?> RouteValues { get; }
}
