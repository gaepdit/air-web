using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Staff.Dto;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.Fces;

public record FceSummaryDto : IDeletable
{
    public int Id { get; init; }

    public string FacilityId { get; init; } = default!;
    public string? FacilityName { get; set; }

    [Display(Name = "FCE Year")]
    public int Year { get; init; }

    // Properties: Deletion
    public bool IsDeleted { get; init; }

    [Display(Name = "Deleted By")]
    public StaffViewDto? DeletedBy { get; init; }

    [Display(Name = "Date Deleted")]
    public DateTimeOffset? DeletedAt { get; init; }

    [Display(Name = "Deletion Comments")]
    public string? DeleteComments { get; init; }
}
