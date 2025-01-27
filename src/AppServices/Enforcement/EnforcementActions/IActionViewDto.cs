using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.EnforcementEntities.Actions;

namespace AirWeb.AppServices.Enforcement.EnforcementActions;

public interface IActionViewDto
{
    public Guid Id { get; }
    public EnforcementActionType ActionType { get; }
    public string Notes { get; }

    // Status
    public EnforcementActionStatus Status { get; }
    public DateOnly? StatusDate { get; }

    // -- Under Review
    public StaffViewDto? CurrentReviewer { get; }
    public DateOnly? ReviewRequestedDate { get; }
    public ICollection<ReviewDto> Reviews { get; }

    // -- Approved
    public DateOnly? ApprovedDate { get; }
    public StaffViewDto? ApprovedBy { get; }

    // -- Issued
    public DateOnly? IssueDate { get; }

    // -- Closed as Unsent
    public DateOnly? ClosedAsUnsentDate { get; }

    // -- Deleted
    public bool IsDeleted { get; }
    public StaffViewDto? DeletedBy { get; }
    public DateTimeOffset? DeletedAt { get; }
}
