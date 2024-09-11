﻿using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using GaEpd.AppLibrary.Extensions;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;

public record WorkEntrySummaryDto : IWorkEntrySummaryDto
{
    public string ItemName => WorkEntryType.GetDescription();
    public FacilityViewDto Facility { get; set; } = default!;
    public string FacilityId { get; init; } = default!;
    public WorkEntryType WorkEntryType { get; init; }

    [Display(Name = "Staff Responsible")]
    public StaffViewDto? ResponsibleStaff { get; init; }

    public bool TrackClosure => WorkEntry.TrackClosure(WorkEntryType);

    // Properties: Closure
    [Display(Name = "Closed")]
    public bool IsClosed { get; init; }

    [Display(Name = "Completed By")]
    public StaffViewDto? ClosedBy { get; init; }

    [Display(Name = "Date Completed")]
    public DateOnly? ClosedDate { get; init; }

    // Properties: Deletion
    public bool IsDeleted { get; init; }
    public StaffViewDto? DeletedBy { get; init; }
    public DateTimeOffset? DeletedAt { get; init; }
    public string? DeleteComments { get; init; }
}
