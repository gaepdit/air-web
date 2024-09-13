using AirWeb.AppServices.Comments;
using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using GaEpd.AppLibrary.Extensions;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;

public record WorkEntryViewDto : IWorkEntryViewDto
{
    protected WorkEntryViewDto() { }

    public int Id { get; init; }
    public string ItemName => WorkEntryType.GetDescription();
    public FacilityViewDto Facility { get; set; } = default!;
    public string FacilityId { get; init; } = default!;
    public WorkEntryType WorkEntryType { get; init; }
    public virtual bool HasPrintout => false;
    public virtual string? PrintoutUrl => null;

    [Display(Name = "Staff Responsible")]
    public StaffViewDto? ResponsibleStaff { get; init; }

    public DateOnly? AcknowledgmentLetterDate { get; init; }
    public string Notes { get; init; } = string.Empty;
    public List<CommentViewDto> Comments { get; } = [];

    // Properties: Closure
    [Display(Name = "Closed")]
    public bool IsClosed { get; init; }

    [Display(Name = "Completed By")]
    public StaffViewDto? ClosedBy { get; init; }

    [Display(Name = "Date Closed")]
    public DateOnly? ClosedDate { get; init; }

    // Properties: Deletion
    public bool IsDeleted { get; init; }
    public StaffViewDto? DeletedBy { get; init; }
    public DateTimeOffset? DeletedAt { get; init; }
    public string? DeleteComments { get; init; }
}
