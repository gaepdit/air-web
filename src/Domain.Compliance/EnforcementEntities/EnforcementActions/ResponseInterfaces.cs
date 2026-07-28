namespace AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;

public interface IResponseRequested : IResponse
{
    public bool ResponseRequested { get; set; }
}

public interface IResponse
{
    public DateOnly? ResponseReceived { get; set; }
    public string? ResponseComment { get; set; }
}
