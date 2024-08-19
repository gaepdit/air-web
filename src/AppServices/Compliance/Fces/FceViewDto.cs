using AirWeb.AppServices.Comments;
using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Staff.Dto;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.Fces;

public record FceViewDto
{
    public int Id { get; init; }

    [Display(Name = "Facility")]
    public FacilityViewDto Facility { get; init; } = default!;

    [Display(Name = "FCE Year")]
    public int Year { get; init; }

    [Display(Name = "Reviewed by")]
    public StaffViewDto ReviewedBy { get; init; } = default!;

    [Display(Name = "Date completed")]
    public DateOnly CompletedDate { get; init; }

    [Display(Name = "With on-site inspection")]
    public bool OnsiteInspection { get; init; }

    [Display(Name = "Notes")]
    public string Notes { get; init; } = string.Empty;

    [Display(Name = "Comments")]
    public List<CommentViewDto> Comments { get; } = [];

    // Properties: Deletion

    [Display(Name = "Deleted?")]
    public bool IsDeleted { get; init; }

    [Display(Name = "Deleted By")]
    public StaffViewDto? DeletedBy { get; init; }

    [Display(Name = "Date Deleted")]
    public DateTimeOffset? DeletedAt { get; init; }

    [Display(Name = "Deletion Comments")]
    public string? DeleteComments { get; init; }
}
