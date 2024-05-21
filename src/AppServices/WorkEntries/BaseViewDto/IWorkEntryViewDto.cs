using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.Entities.Facilities;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.WorkEntries.BaseViewDto;

public interface IWorkEntryViewDto
{
    public int Id { get; }
    public Facility Facility { get; }
    public WorkEntryType WorkEntryType { get; }
    public StaffViewDto? ResponsibleStaff { get; }
    public bool IsClosed { get; }
    public StaffViewDto? ClosedBy { get; }
    public DateOnly? ClosedDate { get; }
    public DateOnly? AcknowledgmentLetterDate { get; }
    public string Notes { get; }

    // Properties: Deletion
    public bool IsDeleted { get; }
    public StaffViewDto? DeletedBy { get; }
    public DateTimeOffset? DeletedAt { get; }
    public string? DeleteComments { get; }

    // Properties: Lists
    public List<Comment> Comments { get; }
}
