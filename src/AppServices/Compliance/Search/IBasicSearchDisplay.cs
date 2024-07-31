namespace AirWeb.AppServices.Compliance.Search;

// FUTURE: See if these will work with enforcement as well.

public interface IBasicSearchDisplay
{
    SortBy Sort { get; }
    IDictionary<string, string?> AsRouteValues();
}
