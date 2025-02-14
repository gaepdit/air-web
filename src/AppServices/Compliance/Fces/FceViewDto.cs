using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.Compliance.Fces;

public record FceViewDto : IFceBasicViewDto
{
    [Display(Name = "FCE tracking number")]
    public int Id { get; init; }

    public string FacilityId { get; init; } = null!;
    public string? FacilityName { get; set; }

    [Display(Name = "FCE Year")]
    public int Year { get; init; }

    [Display(Name = "Reviewed by")]
    public StaffViewDto? ReviewedBy { get; init; }

    [Display(Name = "Date completed")]
    public DateOnly CompletedDate { get; init; }

    [Display(Name = "With on-site inspection")]
    public bool OnsiteInspection { get; init; }

    public string Notes { get; init; } = null!;

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

    // Calculated properties
    public string OwnerId => ReviewedBy?.Id ?? string.Empty;
    public DateOnly SupportingDataStartDate => CompletedDate.AddYears(-Fce.DataPeriod);
    public DateRange SupportingDataDateRange => new(SupportingDataStartDate, CompletedDate);
    public DateOnly ExtendedDataStartDate => CompletedDate.AddYears(-Fce.ExtendedDataPeriod);
}
