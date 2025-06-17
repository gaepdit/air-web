using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.EnforcementEntities.CaseFiles;

namespace AirWeb.AppServices.Enforcement.CaseFileQuery;

public record CaseFileSummaryDto : IIsClosed, IDeletable, IDeleteComments, IHasOwner
{
    public int Id { get; init; }

    public string FacilityId { get; init; } = null!;
    public string? FacilityName { get; set; }

    [Display(Name = "Staff Responsible")]
    public StaffViewDto? ResponsibleStaff { get; init; }

    [Display(Name = "Status")]
    public CaseFileStatus CaseFileStatus { get; init; }

    [Display(Name = "Discovery Date")]
    public DateOnly? DiscoveryDate { get; init; }

    public string Notes { get; init; } = null!;

    // Properties: Closure
    public bool IsClosed { get; init; }

    [Display(Name = "Closed By")]
    public StaffViewDto? ClosedBy { get; init; }

    [Display(Name = "Date Closed")]
    public DateOnly? ClosedDate { get; init; }

    // Properties: Deletion
    public bool IsDeleted { get; init; }

    [Display(Name = "Deleted By")]
    public StaffViewDto? DeletedBy { get; init; }

    [Display(Name = "Date Deleted")]
    public DateTimeOffset? DeletedAt { get; init; }

    [Display(Name = "Deletion Comments")]
    public string? DeleteComments { get; init; }

    // Calculated properties
    public string OwnerId => ResponsibleStaff?.Id ?? string.Empty;
}
