using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Staff.Dto;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.Fces;

public record FceViewDto : IHasOwnerAndDeletable
{
    public int Id { get; init; }

    public FacilityViewDto Facility { get; init; } = default!;

    [Display(Name = "FCE Year")]
    public int Year { get; init; }

    [Display(Name = "Reviewed by")]
    public StaffViewDto ReviewedBy { get; init; } = default!;

    [Display(Name = "Date completed")]
    public DateOnly CompletedDate { get; init; }

    [Display(Name = "With on-site inspection")]
    public bool OnsiteInspection { get; init; }

    public string Notes { get; init; } = string.Empty;

    [UsedImplicitly]
    public List<CommentViewDto> Comments { get; } = [];

    // Properties: Deletion
    public bool IsDeleted { get; init; }

    [Display(Name = "Deleted by")]
    public StaffViewDto? DeletedBy { get; init; }

    [Display(Name = "Date deleted")]
    public DateTimeOffset? DeletedAt { get; init; }

    [Display(Name = "Deletion Comments")]
    public string? DeleteComments { get; init; }

    public string OwnerId => ReviewedBy.Id;
}
