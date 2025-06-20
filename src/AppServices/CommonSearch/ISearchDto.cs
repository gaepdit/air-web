namespace AirWeb.AppServices.CommonSearch;

public interface ISearchDto<out TSelf> : ISorting, IRouteValues
    where TSelf : ISearchDto<TSelf>

{
    public TSelf TrimAll();
}

public interface ISearchDto : ISorting, IRouteValues;

public interface ISorting
{
    public string SortByName { get; }
    public string Sorting { get; }
}

public interface IRouteValues
{
    IDictionary<string, string?> AsRouteValues();
}

public interface IDeleteStatus
{
    DeleteStatus? DeleteStatus { get; set; }
}
