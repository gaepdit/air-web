using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.EnforcementEntities.Actions;

namespace AirWeb.AppServices.Enforcement.Query;

public record EnforcementActionViewDto
{
    public Guid Id { get; init; }
    public EnforcementActionType ActionType { get; init; }
    public string Notes { get; init; } = null!;
    public StaffViewDto? ResponsibleStaff { get; set; }

    // Status

    // -- Issued
    public DateOnly? IssueDate { get; internal set; }
    public bool IsIssued => IssueDate.HasValue;

    // -- Approved
    public bool IsApproved { get; internal set; }
    public DateOnly? ApprovedDate { get; internal set; }
    public StaffViewDto? ApprovedBy { get; set; }

    // -- Under Review
    public StaffViewDto? CurrentReviewer { get; set; }
    public DateOnly? ReviewRequestedDate { get; set; }
    public ICollection<ReviewDto> Reviews { get; } = [];

    // -- Closed as Unsent
    public DateOnly? ClosedAsUnsent { get; internal set; }
    public bool IsClosedAsUnsent => ClosedAsUnsent.HasValue;

    // -- Deleted
    public bool IsDeleted { get; init; }

    [Display(Name = "Deleted by")]
    public StaffViewDto? DeletedBy { get; init; }

    [Display(Name = "Date deleted")]
    public DateTimeOffset? DeletedAt { get; init; }

    [Display(Name = "Deletion Comments")]
    public string? DeleteComments { get; init; }
}
