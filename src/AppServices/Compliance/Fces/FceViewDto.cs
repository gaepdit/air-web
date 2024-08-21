using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Staff.Dto;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.Fces;

public record FceViewDto : IDeletedItem
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

    public required string Notes { get; init; }

    [UsedImplicitly]
    public List<CommentViewDto> Comments { get; } = [];

    // Properties: Deletion
    public string ItemName => "FCE";
    public string ItemId => Id.ToString();
    public bool IsDeleted { get; init; }
    public StaffViewDto? DeletedBy { get; init; }
    public DateTimeOffset? DeletedAt { get; init; }
    public string? DeleteComments { get; init; }
}
