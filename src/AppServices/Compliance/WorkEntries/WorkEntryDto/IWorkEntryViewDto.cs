using AirWeb.AppServices.Comments;
using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;

public interface IWorkEntryViewDto
{
    public int Id { get; }
    public FacilityViewDto Facility { get; set; }
    public string FacilityId { get; }
    public WorkEntryType WorkEntryType { get; }
    public bool HasPrintout { get; }
    public string? PrintoutUrl { get; }

    [Display(Name = "Staff Responsible")]
    public StaffViewDto? ResponsibleStaff { get; }

    [Display(Name = "Date Acknowledgment Letter Sent")]
    public DateOnly? AcknowledgmentLetterDate { get; }

    public string Notes { get; }
    public List<CommentViewDto> Comments { get; }

    // Properties: Closure
    [Display(Name = "Closed")]
    public bool IsClosed { get; }

    [Display(Name = "Completed By")]
    public StaffViewDto? ClosedBy { get; }

    [Display(Name = "Date Closed")]
    public DateOnly? ClosedDate { get; }

    // Properties: Deletion
    public bool IsDeleted { get; }
    public StaffViewDto? DeletedBy { get; }
    public DateTimeOffset? DeletedAt { get; }
    public string? DeleteComments { get; }
}
