using AirWeb.Domain.Identity;

namespace AirWeb.Domain.BaseEntities;

public abstract class ClosableEntity<TKey> : DeletableEntity<TKey>
    where TKey : IEquatable<TKey>
{
    // Closure properties
    public bool IsClosed => ClosedDate is not null;
    public ApplicationUser? ClosedBy { get; internal set; }
    public DateOnly? ClosedDate { get; internal set; }

    internal void Close(ApplicationUser? user)
    {
        ClosedDate = DateOnly.FromDateTime(DateTime.Now);
        ClosedBy = user;
    }

    internal void Reopen()
    {
        ClosedDate = null;
        ClosedBy = null;
    }
}
