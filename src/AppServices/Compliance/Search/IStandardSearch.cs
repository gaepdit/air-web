namespace AirWeb.AppServices.Compliance.Search;

// FUTURE: See if these will work with enforcement as well.

public interface IStandardSearch
{
    SortBy Sort { get; }
    public DeleteStatus? DeleteStatus { get; set; }
    IDictionary<string, string?> AsRouteValues();
}
