using AirWeb.Domain.EnforcementEntities.Actions;
using AirWeb.Domain.Identity;
using System.ComponentModel;

namespace AirWeb.Domain.EnforcementEntities.ActionProperties;

public class EnforcementActionReview : AuditableEntity
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private EnforcementActionReview() { }

    internal EnforcementActionReview(Guid id, EnforcementAction enforcementAction, ApplicationUser? user)
    {
        Id = id;
        EnforcementAction = enforcementAction;
        SetCreator(user?.Id);
    }

    public EnforcementAction EnforcementAction { get; internal init; } = null!;

    public DateOnly DateRequested { get; internal init; }
    public ApplicationUser? ReviewedBy { get; internal init; }
    public bool IsCompleted => DateCompleted.HasValue;
    public DateOnly? DateCompleted { get; internal set; }
    public ReviewResult? Status { get; internal set; }

    [StringLength(7000)]
    public string? ReviewComments { get; internal set; }
}

public enum ReviewResult
{
    // Returned to staff with changes requested.
    [Description("Changes requested")] Returned,

    // Forwarded to next level of management for approval.
    [Description("Forwarded for additional review")] Forwarded,

    // Approved with no further review required.
    [Description("Approved")] Approved,

    // Disapproved.
    [Description("Disapproved")] Disapproved,
}
