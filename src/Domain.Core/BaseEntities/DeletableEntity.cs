using AirWeb.Domain.Core.Entities;

namespace AirWeb.Domain.Core.BaseEntities;

public abstract class DeletableEntity<TKey> : AuditableSoftDeleteEntity<TKey>, IIsDeleted
    where TKey : IEquatable<TKey>
{
    // Deletion properties
    public ApplicationUser? DeletedBy { get; set; }

    [StringLength(7000)]
    public string? DeleteComments { get; set; }

    // Deletion methods
    public void Delete(string? comment, ApplicationUser? user)
    {
        SetDeleted(user?.Id);
        DeletedBy = user;
        DeleteComments = comment;
    }

    public void Undelete()
    {
        SetNotDeleted();
        DeletedBy = null;
        DeleteComments = null;
    }
}
