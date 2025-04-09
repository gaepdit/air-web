using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Enforcement.EnforcementActionQuery;

public record ActionViewDto : IActionViewDto
{
    public Guid Id { get; init; }
    public int CaseFileId { get; init; }
    public EnforcementActionType ActionType { get; init; }
    public string Notes { get; init; } = null!;

    // Status
    public EnforcementActionStatus Status { get; init; }

    public DateOnly? StatusDate => Status switch
    {
        EnforcementActionStatus.ReviewRequested => ReviewRequestedDate,
        EnforcementActionStatus.Approved => ApprovedDate,
        EnforcementActionStatus.Issued => IssueDate,
        EnforcementActionStatus.Canceled => CanceledDate,
        EnforcementActionStatus.Draft => DateOnly.FromDateTime(CreatedAt.DateTime),
        _ => null,
    };

    public bool IsReportable { get; init; }
    public bool WillBeReportable { get; init; }
    public DateTimeOffset CreatedAt { get; init; }

    // -- Under Review
    public StaffViewDto? CurrentReviewer { get; init; }
    public DateOnly? ReviewRequestedDate { get; init; }
    public ICollection<ReviewDto> Reviews { get; } = [];

    // -- Approved
    public DateOnly? ApprovedDate { get; init; }
    public StaffViewDto? ApprovedBy { get; init; }
    public bool IsApproved => ApprovedDate.HasValue;

    // -- Issued
    public DateOnly? IssueDate { get; init; }
    public bool IsIssued => IssueDate.HasValue;

    // -- Canceled (closed as unsent)
    public DateOnly? CanceledDate { get; init; }
    public bool IsCanceled => CanceledDate.HasValue;

    // -- Deleted
    public bool IsDeleted { get; init; }

    [Display(Name = "Deleted by")]
    public StaffViewDto? DeletedBy { get; init; }

    [Display(Name = "Date deleted")]
    public DateTimeOffset? DeletedAt { get; init; }
}
