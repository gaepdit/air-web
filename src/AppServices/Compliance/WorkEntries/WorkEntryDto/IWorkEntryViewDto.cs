using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;

public interface IWorkEntryViewDto : IDeletedItem
{
    public int Id { get; }
    public FacilityViewDto Facility { get; set; }
    public string FacilityId { get; }
    public WorkEntryType WorkEntryType { get; }

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

    // Display properties
    public bool HasPrintout { get; }
    public string? PrintoutUrl { get; }
}
