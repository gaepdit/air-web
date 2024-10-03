using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using GaEpd.AppLibrary.Extensions;
using IaipDataService.Facilities;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;

public record WorkEntrySummaryDto : IWorkEntrySummaryDto
{
    public string ItemName => WorkEntryType.GetDescription();
    public Facility Facility { get; set; } = default!;
    public string FacilityId { get; init; } = default!;
    public WorkEntryType WorkEntryType { get; init; }

    [Display(Name = "Staff Responsible")]
    public StaffViewDto? ResponsibleStaff { get; init; }

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
}
