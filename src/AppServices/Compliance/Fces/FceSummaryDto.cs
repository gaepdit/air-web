using AirWeb.AppServices.Core.EntityServices.Staff.Dto;

namespace AirWeb.AppServices.Compliance.Fces;

public record FceSummaryDto : IFceBasicViewDto
{
    public int Id { get; init; }

    public string FacilityId { get; init; } = null!;
    public string? FacilityName { get; set; }

    [Display(Name = "FCE Year")]
    public int Year { get; init; }

    [Display(Name = "Reviewed By")]
    public StaffViewDto? ReviewedBy { get; init; }

    public DateOnly CompletedDate { get; init; }

    [Display(Name = "With On-Site Inspection")]
    public bool OnsiteInspection { get; init; }

    public string Notes { get; init; } = null!;

    // Properties: Deletion
    public bool IsDeleted { get; init; }

    [Display(Name = "Deleted By")]
    public StaffViewDto? DeletedBy { get; init; }

    [Display(Name = "Date Deleted")]
    public DateTimeOffset? DeletedAt { get; init; }

    [Display(Name = "Deletion Comments")]
    public string? DeleteComments { get; init; }

    // Calculated properties

    // Not used for FCE summary but required by the interface.
    public string OwnerId => string.Empty;
}
