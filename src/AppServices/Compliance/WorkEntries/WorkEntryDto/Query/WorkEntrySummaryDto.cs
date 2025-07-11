﻿using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using GaEpd.AppLibrary.Extensions;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;

public record WorkEntrySummaryDto : IWorkEntrySummaryDto
{
    public string ItemName => WorkEntryType.GetDisplayName();
    public string FacilityId { get; init; } = null!;
    public string? FacilityName { get; set; }
    public WorkEntryType WorkEntryType { get; init; }
    public bool IsComplianceEvent { get; init; }

    [Display(Name = "Staff Responsible")]
    public StaffViewDto? ResponsibleStaff { get; init; }

    public DateOnly EventDate { get; init; }
    public string EventDateName { get; init; } = string.Empty;

    // Properties: Closure
    [Display(Name = "Closed")]
    public bool IsClosed { get; init; }

    [Display(Name = "Completed By")]
    public StaffViewDto? ClosedBy { get; init; }

    [Display(Name = "Date Completed")]
    public DateOnly? ClosedDate { get; init; }

    // Properties: Deletion
    public bool IsDeleted { get; init; }

    [Display(Name = "Deleted By")]
    public StaffViewDto? DeletedBy { get; init; }

    [Display(Name = "Date Deleted")]
    public DateTimeOffset? DeletedAt { get; init; }

    [Display(Name = "Deletion Comments")]
    public string? DeleteComments { get; init; }

    public string OwnerId => ResponsibleStaff?.Id ?? string.Empty;
}
