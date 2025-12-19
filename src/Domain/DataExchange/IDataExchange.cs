namespace AirWeb.Domain.DataExchange;

public interface IDataExchange
{
    // public string FacilityId { get; }
    public ushort? ActionNumber { get; }
    public DataExchangeStatus DataExchangeStatus { get; }
    public DateTimeOffset DataExchangeStatusDate { get; }
    public bool DataExchangeExempt { get; }
}
