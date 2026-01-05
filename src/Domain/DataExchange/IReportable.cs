namespace AirWeb.Domain.DataExchange;

public interface IReportable
{
    short ActionNumber { get; }
    DataExchangeStatus DataExchangeStatus { get; }
}
