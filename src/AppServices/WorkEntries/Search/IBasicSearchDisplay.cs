namespace AirWeb.AppServices.WorkEntries.Search;

public interface IBasicSearchDisplay
{
    SortBy Sort { get; }
    IDictionary<string, string?> AsRouteValues();
}
