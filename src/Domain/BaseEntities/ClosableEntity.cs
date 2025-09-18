using AirWeb.Domain.Identity;
using System.Diagnostics.CodeAnalysis;

namespace AirWeb.Domain.BaseEntities;

public interface IIsClosed
{
    public bool IsClosed { get; }
}

public abstract class ClosableEntity<TKey> : DeletableEntity<TKey>, IIsClosed
    where TKey : IEquatable<TKey>
{
    // Closure properties
    public bool IsClosed
    {
        get => ClosedDate.HasValue;

        [UsedImplicitly]
        [SuppressMessage("ReSharper", "ValueParameterNotUsed")]
        [SuppressMessage("Blocker Code Smell", "S3237:\"value\" contextual keyword should be used")]
        private set
        {
            // Method intentionally left empty: This allows storing read-only properties in the database.
            // See: https://github.com/dotnet/efcore/issues/13316#issuecomment-421052406
        }
    }

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
