using AirWeb.AppServices.Compliance.Search;

namespace AirWeb.AppServices.CommonSearch;

public interface ISearchDto : ISortByName, IRouteValues;

public interface ISortByName
{
    public string SortByName { get; }
}

public interface IRouteValues
{
    IDictionary<string, string?> AsRouteValues();
}

public interface IDeleteStatus
{
    DeleteStatus? DeleteStatus { get; set; }
}
