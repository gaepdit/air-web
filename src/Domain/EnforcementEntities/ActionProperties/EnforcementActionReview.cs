using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.ActionProperties;

public class EnforcementActionReview : AuditableEntity
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private EnforcementActionReview() { }

    internal EnforcementActionReview(Guid id, EnforcementAction enforcementAction, ApplicationUser reviewer,
        ApplicationUser? user)
    {
        Id = id;
        EnforcementAction = enforcementAction;
        RequestedTo = reviewer;
        SetCreator(user?.Id);
    }

    public EnforcementAction EnforcementAction { get; internal init; } = null!;

    public DateOnly RequestedDate { get; internal init; }
    public ApplicationUser RequestedTo { get; internal init; } = null!;
    public ApplicationUser? ReviewedBy { get; internal init; }
    public bool IsCompleted => CompletedDate.HasValue;
    public DateOnly? CompletedDate { get; internal set; }

    [StringLength(11)]
    public ReviewResult? Result { get; internal set; }

    [StringLength(7000)]
    public string? ReviewComments { get; internal set; }
}

public enum ReviewResult
{
    // Returned to staff with changes requested.
    [Display(Name = "Changes requested")] Returned,

    // Forwarded to the next level of management for approval.
    [Display(Name = "Forwarded for additional review")] Forwarded,

    // Approved with no further review required.
    [Display(Name = "Approved")] Approved,

    // Disapproved.
    [Display(Name = "Disapproved")] Disapproved,
}
