using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Staff.Dto;
using IaipDataService.Facilities;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.Fces;

public record FceSummaryDto : IDeletable
{
    public int Id { get; init; }

    public Facility Facility { get; init; } = default!;

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
