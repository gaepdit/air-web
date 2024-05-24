using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.Entities.Facilities;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.WorkEntries.BaseWorkEntryDto;

public interface IWorkEntryViewDto
{
    public int Id { get; }
    public Facility Facility { get; }
    public WorkEntryType WorkEntryType { get; }
    public ComplianceEventType ComplianceEventType { get; }
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
