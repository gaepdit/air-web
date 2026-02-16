using AirWeb.AppServices.Core.EntityServices.Staff.Dto;
using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Compliance.Enforcement.EnforcementActionQuery;

public record ActionViewDto : IActionViewDto
{
    public Guid Id { get; init; }
    public int CaseFileId { get; init; }
    public StaffViewDto? CaseFileResponsibleStaff { get; init; }

    public EnforcementActionType ActionType { get; init; }
    public string Notes { get; init; } = null!;

    // Status
    public EnforcementActionStatus Status { get; init; }

    public DateOnly? StatusDate => Status switch
    {
        EnforcementActionStatus.ReviewRequested => DateOnly.FromDateTime(ReviewRequestedDate!.Value),
        EnforcementActionStatus.Approved => DateOnly.FromDateTime(ApprovedDate!.Value),
        EnforcementActionStatus.Issued => IssueDate,
        EnforcementActionStatus.Canceled => DateOnly.FromDateTime(CanceledDate!.Value),
        EnforcementActionStatus.Draft => CreatedAt is null ? null : DateOnly.FromDateTime(CreatedAt.Value.DateTime),
        _ => null,
    };

    public bool IsReportable { get; init; }
    public bool IsReportableAction { get; init; }
    public DateTimeOffset? CreatedAt { get; init; }

    // -- Under Review
    public StaffViewDto? CurrentReviewer { get; init; }
    public ReviewDto? CurrentOpenReview { get; init; }
    public DateTime? ReviewRequestedDate { get; init; }
    public ICollection<ReviewDto> Reviews { get; } = [];

    // -- Approved
    public DateTime? ApprovedDate { get; init; }
    public StaffViewDto? ApprovedBy { get; init; }
    public bool IsApproved => ApprovedDate.HasValue;

    // -- Issued
    public DateOnly? IssueDate { get; init; }
    public bool IsIssued => IssueDate.HasValue;

    // -- Canceled (closed as unsent)
    public DateTime? CanceledDate { get; init; }
    public bool IsCanceled => CanceledDate.HasValue;

    // -- Deleted
    public bool IsDeleted { get; init; }

    [Display(Name = "Deleted By")]
    public StaffViewDto? DeletedBy { get; init; }

    [Display(Name = "Date Deleted")]
    public DateTimeOffset? DeletedAt { get; init; }
}
