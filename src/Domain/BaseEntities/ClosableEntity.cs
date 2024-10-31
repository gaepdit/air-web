using AirWeb.Domain.Identity;

namespace AirWeb.Domain.BaseEntities;

public abstract class ClosableEntity<TKey> : DeletableEntity<TKey>
    where TKey : IEquatable<TKey>
{
    // Closure properties
    public bool IsClosed { get; internal set; }
    public ApplicationUser? ClosedBy { get; internal set; }
    public DateOnly? ClosedDate { get; internal set; }

    internal void Close(ApplicationUser? user)
    {
        IsClosed = true;
        ClosedDate = DateOnly.FromDateTime(DateTime.Now);
        ClosedBy = user;
    }

    internal void Reopen()
    {
        IsClosed = false;
        ClosedDate = null;
        ClosedBy = null;
    }
}
