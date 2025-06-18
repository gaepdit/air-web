using AirWeb.AppServices.Compliance.Search;

namespace AirWeb.AppServices.CommonSearch;

public interface ISearchDto
{
    SortBy Sort { get; }
    DeleteStatus? DeleteStatus { get; set; }
    IDictionary<string, string?> AsRouteValues();
}
