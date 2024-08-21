using AirWeb.AppServices.Comments;
using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using GaEpd.AppLibrary.Extensions;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;

public record WorkEntryViewDto : IWorkEntryViewDto
{
    protected WorkEntryViewDto() { }

    public int Id { get; init; }
    public FacilityViewDto Facility { get; set; } = default!;
    public string FacilityId { get; init; } = default!;
    public WorkEntryType WorkEntryType { get; init; }
    public virtual bool HasPrintout => false;
    public virtual string? PrintoutUrl => null;
    public StaffViewDto? ResponsibleStaff { get; init; }
    public DateOnly? AcknowledgmentLetterDate { get; init; }
    public required string Notes { get; init; }
    public List<CommentViewDto> Comments { get; } = [];

    // Properties: Closure
    public bool IsClosed { get; init; }
    public StaffViewDto? ClosedBy { get; init; }
    public DateOnly? ClosedDate { get; init; }

    // Properties: Deletion
    public string ItemName => WorkEntryType.GetDescription();
    public string ItemId => Id.ToString();
    public bool IsDeleted { get; init; }
    public StaffViewDto? DeletedBy { get; init; }
    public DateTimeOffset? DeletedAt { get; init; }
    public string? DeleteComments { get; init; }
}
