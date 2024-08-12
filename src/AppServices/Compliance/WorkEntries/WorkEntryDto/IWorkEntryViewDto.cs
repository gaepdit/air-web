using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;

public interface IWorkEntryViewDto
{
    public int Id { get; }
    public FacilityViewDto Facility { get; set; }
    public string FacilityId { get; }
    public WorkEntryType WorkEntryType { get; }
    public StaffViewDto? ResponsibleStaff { get; }
    public DateOnly? AcknowledgmentLetterDate { get; }
    public string Notes { get; }

    // Properties: Lists
    public List<Comment> Comments { get; }

    // Properties: Closure
    public bool IsClosed { get; }
    public StaffViewDto? ClosedBy { get; }
    public DateOnly? ClosedDate { get; }

    // Properties: Deletion
    public bool IsDeleted { get; }
    public StaffViewDto? DeletedBy { get; }
    public DateTimeOffset? DeletedAt { get; }
    public string? DeleteComments { get; }
}
