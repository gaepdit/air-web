using AirWeb.Domain.Identity;

namespace AirWeb.Domain.BaseEntities;

public interface IIsClosed
{
    public bool IsClosed { get; }
}

public abstract class ClosableEntity<TKey> : DeletableEntity<TKey>, IIsClosed
    where TKey : IEquatable<TKey>
{
    // Closure properties
    public bool IsClosed => ClosedDate.HasValue;
    public ApplicationUser? ClosedBy { get; internal set; }
    public DateOnly? ClosedDate { get; internal set; }

    internal void Close(ApplicationUser? user)
    {
        ClosedDate = DateOnly.FromDateTime(DateTime.Today);
        ClosedBy = user;
        SetUpdater(user?.Id);
    }

    internal void Reopen(ApplicationUser? user)
    {
        ClosedDate = null;
        ClosedBy = null;
        SetUpdater(user?.Id);
    }
}
