namespace AirWeb.Domain.EnforcementEntities.Actions;

public interface IResponseRequested
{
    public bool ResponseRequested { get; }
    public DateOnly? ResponseReceived { get; }
    public string? ResponseComment { get; }
}
