using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.WorkEntries.ViewDto;

public interface IWorkEntryViewDto
{
    public int Id { get; }

    public DateTimeOffset ReceivedDate { get; }

    public StaffViewDto? ReceivedBy { get; }

    public string? EntryTypeName { get; }

    public string Notes { get; }

    // Properties: Review/Closure

    public bool Closed { get; }

    public DateTimeOffset? ClosedDate { get; }

    public StaffViewDto? ClosedBy { get; }

    public string? ClosedComments { get; }

    // Properties: Deletion

    public bool IsDeleted { get; }

    public StaffViewDto? DeletedBy { get; }

    public DateTimeOffset? DeletedAt { get; }

    public string? DeleteComments { get; }

    // === Lists ===

    public List<Comment> Comments { get; }
}
