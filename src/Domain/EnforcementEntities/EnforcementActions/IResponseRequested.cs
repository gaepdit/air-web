namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public interface IResponseRequested
{
    public void RequestResponse();
    public bool ResponseRequested { get; }
    public DateOnly? ResponseReceived { get; set; }
    public string? ResponseComment { get; set; }
}

public interface IResponseRequestedSetter
{
    public bool ResponseRequested { get; set; }
}
