using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.Fces;

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

    [Display(Name = "Notes")]
    public string Notes { get; init; } = string.Empty;

    [Display(Name = "Comments")]
    public List<Comment> Comments { get; } = [];
}
