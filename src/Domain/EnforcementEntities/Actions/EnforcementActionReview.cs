using AirWeb.Domain.Identity;
using System.ComponentModel;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class EnforcementActionReview : AuditableEntity
{
    public EnforcementAction EnforcementAction { get; internal init; } = null!;
    public ApplicationUser? ReviewedBy { get; internal init; }
    public DateOnly DateRequested { get; internal init; }
    public bool Completed { get; internal set; }
    public DateOnly? DateCompleted { get; internal set; }
    public EnforcementActionReviewStatus Status { get; internal set; } = EnforcementActionReviewStatus.InProgress;

    [StringLength(7000)]
    public string? ReviewComments { get; internal set; }
}

public enum EnforcementActionReviewStatus
{
    // Review requested.
    [Description("Review requested")] InProgress,

    // Returned to staff with changes requested.
    [Description("Changes requested")] Returned,

    // Forwarded to next level of management for approval.
    [Description("Forwarded for additional review")] Forwarded,

    // Approved with no further review required.
    [Description("Approved")] Approved,

    // Disapproved.
    [Description("Disapproved")] Disapproved,
}
