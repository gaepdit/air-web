namespace AirWeb.AppServices.CommonSearch;

public interface IDeleteStatus
{
    DeleteStatus? DeleteStatus { get; set; }
}

public interface IClosedStatus
{
    ClosedOpenAny? Closed { get; init; }
}
