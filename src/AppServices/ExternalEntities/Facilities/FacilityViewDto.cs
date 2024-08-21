using AirWeb.Domain.ExternalEntities.Facilities;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.ExternalEntities.Facilities;

public record FacilityViewDto
{
    [Display(Name = "Facility ID")]
    public FacilityId Id { get; init; } = default!;

    [Display(Name = "Company name")]
    public required string CompanyName { get; init; }

    [Display(Name = "Facility description")]
    public required string Description { get; init; }
}
