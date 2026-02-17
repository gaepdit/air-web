using AirWeb.Domain.Core.Entities;
using System.ComponentModel;

namespace AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions.ActionProperties;

public class EnforcementActionReview : AuditableEntity
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private EnforcementActionReview() { }

    internal EnforcementActionReview(Guid id, EnforcementAction enforcementAction, ApplicationUser reviewer,
        ApplicationUser requester)
    {
        Id = id;
        EnforcementAction = enforcementAction;
        RequestedDate = DateTime.Today;
        RequestedBy = requester;
        RequestedOf = reviewer;
        SetCreator(requester.Id);
    }

    public EnforcementAction EnforcementAction { get; internal init; } = null!;

    public DateTime RequestedDate { get; internal init; }

    public ApplicationUser RequestedBy { get; internal init; } = null!;
    public ApplicationUser RequestedOf { get; internal init; } = null!;
    public ApplicationUser? ReviewedBy { get; internal set; }

    public bool IsCompleted => CompletedDate.HasValue;
    public DateTime? CompletedDate { get; internal set; }

    [StringLength(11)]
    public ReviewResult? Result { get; internal set; }

    [StringLength(7000)]
    public string? ReviewComments { get; internal set; }

    internal void CompleteReview(ApplicationUser user, ReviewResult result, string? comments)
    {
        ReviewedBy = user;
        Result = result;
        ReviewComments = comments;
        CompletedDate = DateTime.Today;
    }
}

public enum ReviewResult
{
    // Approved with no further review required.
    [Display(Name = "Approved")]
    [Description("Approve")]
    Approved,

    // Returned to staff with changes requested.
    [Display(Name = "Changes requested")]
    [Description("Request changes")]
    Returned,

    // Canceled/Disapproved.
    [Display(Name = "Canceled")]
    [Description("Cancel and close as unsent")]
    Canceled,

    // Forwarded to someone else for additional review.
    [Display(Name = "Forwarded for additional review")]
    [Description("Approve and forward for additional review")]
    Forwarded,
}
