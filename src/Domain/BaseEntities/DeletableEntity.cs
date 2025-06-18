using AirWeb.Domain.BaseEntities.Interfaces;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.BaseEntities;

public abstract class DeletableEntity<TKey> : AuditableSoftDeleteEntity<TKey>, IIsDeleted
    where TKey : IEquatable<TKey>
{
    // Deletion properties
    public ApplicationUser? DeletedBy { get; set; }

    [StringLength(7000)]
    public string? DeleteComments { get; set; }

    // Deletion methods
    internal void Delete(string? comment, ApplicationUser? user)
    {
        SetDeleted(user?.Id);
        DeletedBy = user;
        DeleteComments = comment;
    }

    internal void Undelete()
    {
        SetNotDeleted();
        DeletedBy = null;
        DeleteComments = null;
    }
}
