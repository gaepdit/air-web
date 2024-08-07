using AirWeb.Domain.ExternalEntities.Facilities;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.ExternalEntities.Facilities;

public record FacilityViewDto
{
    [Display(Name = "Facility ID")]
    public FacilityId Id { get; init; } = default!;

    [Display(Name = "Company name")]
    public string CompanyName { get; init; } = string.Empty;

    [Display(Name = "Facility description")]
    public string Description { get; init; } = string.Empty;
}
