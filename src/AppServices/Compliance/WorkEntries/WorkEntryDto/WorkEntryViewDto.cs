using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;

public record WorkEntryViewDto : IWorkEntryViewDto
{
    protected WorkEntryViewDto() { }

    public int Id { get; init; }
    public FacilityViewDto Facility { get; set; } = default!;
    public string FacilityId { get; init; } = default!;
    public WorkEntryType WorkEntryType { get; init; }

    [Display(Name = "Staff Responsible")]
    public StaffViewDto? ResponsibleStaff { get; init; }

    [Display(Name = "Closed")]
    public bool IsClosed { get; init; }

    [Display(Name = "Date Closed")]
    public DateOnly? ClosedDate { get; init; }

    [Display(Name = "Closed By")]
    public StaffViewDto? ClosedBy { get; init; }

    [Display(Name = "Date Acknowledgment Letter Sent")]
    public DateOnly? AcknowledgmentLetterDate { get; init; }

    [Display(Name = "Notes")]
    public string Notes { get; init; } = string.Empty;

    // Properties: Deletion

    [Display(Name = "Deleted?")]
    public bool IsDeleted { get; init; }

    [Display(Name = "Deleted By")]
    public StaffViewDto? DeletedBy { get; init; }

    [Display(Name = "Date Deleted")]
    public DateTimeOffset? DeletedAt { get; init; }

    [Display(Name = "Comments")]
    public string? DeleteComments { get; init; }

    // Properties: Lists
    public List<Comment> Comments { get; } = [];
}
