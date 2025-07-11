﻿using AirWeb.AppServices.DtoInterfaces;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Enforcement.EnforcementActionQuery;

public interface IActionViewDto : IDeletable
{
    public Guid Id { get; }
    public int CaseFileId { get; }
    public EnforcementActionType ActionType { get; }
    public string Notes { get; }

    // Status
    public EnforcementActionStatus Status { get; }
    public DateOnly? StatusDate { get; }
    public bool IsReportable { get; }
    public bool WillBeReportable { get; }
    public DateTimeOffset CreatedAt { get; }

    // -- Under Review
    public StaffViewDto? CurrentReviewer { get; }
    public DateOnly? ReviewRequestedDate { get; }
    public ICollection<ReviewDto> Reviews { get; }

    // -- Approved
    public DateOnly? ApprovedDate { get; }
    public StaffViewDto? ApprovedBy { get; }
    public bool IsApproved { get; }

    // -- Issued
    public DateOnly? IssueDate { get; }
    public bool IsIssued { get; }

    // -- Canceled (closed as unsent)
    public DateOnly? CanceledDate { get; }
    public bool IsCanceled { get; }
}
