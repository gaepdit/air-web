using AirWeb.AppServices.Facilities;
using AirWeb.AppServices.Staff.Dto;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Fces;

public record FceViewDto
{
    [Display(Name = "Facility")]
    public FacilityViewDto Facility { get; init; } = default!;

    [Display(Name = "FCE Year")]
    public int Year { get; init; }

    [Display(Name = "Reviewed by")]
    public StaffViewDto ReviewedBy { get; init; } = default!;

    [Display(Name = "Date Completed")]
    public DateOnly CompletedDate { get; init; }

    [Display(Name = "With on-site inspection")]
    public bool OnsiteInspection { get; init; }

    [Display(Name = "Initial comments")]
    public string Comment { get; init; } = string.Empty;
}
