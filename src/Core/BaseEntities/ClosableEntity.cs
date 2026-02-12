using AirWeb.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace AirWeb.Core.BaseEntities;

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

    public ApplicationUser? ClosedBy { get; set; }
    public DateOnly? ClosedDate { get; set; }

    public void Close(ApplicationUser? user)
    {
        ClosedDate = DateOnly.FromDateTime(DateTime.Today);
        ClosedBy = user;
        SetUpdater(user?.Id);
    }

    public void Reopen(ApplicationUser? user)
    {
        ClosedDate = null;
        ClosedBy = null;
        SetUpdater(user?.Id);
    }
}
