namespace AirWeb.AppServices.WorkEntries.SearchDto;

public interface IBasicSearchDisplay
{
    SortBy Sort { get; }
    IDictionary<string, string?> AsRouteValues();
}
