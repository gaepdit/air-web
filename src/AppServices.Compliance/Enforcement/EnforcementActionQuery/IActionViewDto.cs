using AirWeb.AppServices.Compliance.DtoInterfaces;
using AirWeb.AppServices.Core.EntityServices.Staff.Dto;
using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Compliance.Enforcement.EnforcementActionQuery;

public interface IActionViewDto : IDeletable
{
    public Guid Id { get; }
    public int CaseFileId { get; }
    public StaffViewDto? CaseFileResponsibleStaff { get; }

    public EnforcementActionType ActionType { get; }
    public string Notes { get; }

    // Status
    public EnforcementActionStatus Status { get; }
    public DateOnly? StatusDate { get; }

    // Metadata
    public bool IsReportableAction { get; }
    public DateTimeOffset? CreatedAt { get; }

    // -- Under Review
    public StaffViewDto? CurrentReviewer { get; }
    public ReviewDto? CurrentOpenReview { get; }
    public bool IsUnderReview => Status == EnforcementActionStatus.ReviewRequested;
    public ICollection<ReviewDto> Reviews { get; }

    // -- Approved
    public DateTime? ApprovedDate { get; }

    // -- Issued
    public DateOnly? IssueDate { get; }
    public bool IsIssued => Status == EnforcementActionStatus.Issued;

    // -- Canceled (closed as unsent)
    public DateTime? CanceledDate { get; }
    public bool IsCanceled => Status == EnforcementActionStatus.Canceled;
}
