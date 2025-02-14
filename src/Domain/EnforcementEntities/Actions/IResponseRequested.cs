namespace AirWeb.Domain.EnforcementEntities.Actions;

public interface IResponseRequested
{
    public void RequestResponse();
    public bool ResponseRequested { get; }
    public DateOnly? ResponseReceived { get; }
    public string? ResponseComment { get; }
}
