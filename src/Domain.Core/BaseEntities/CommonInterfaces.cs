namespace AirWeb.Domain.Core.BaseEntities;

public interface IIsClosed
{
    public bool IsClosed { get; }
}

public interface IIsDeleted
{
    public bool IsDeleted { get; }
}

public interface INotes
{
    public string? Notes { get; }
}
