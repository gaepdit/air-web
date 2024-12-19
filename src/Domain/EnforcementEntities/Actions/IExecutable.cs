namespace AirWeb.Domain.EnforcementEntities.Actions;

public interface IExecutable
{
    public bool IsExecuted { get; }
    public bool IsResolved { get; }
}
