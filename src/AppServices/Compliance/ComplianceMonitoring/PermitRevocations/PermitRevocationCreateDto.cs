using AirWeb.AppServices.Compliance.ComplianceMonitoring.WorkEntryDto.Command;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.PermitRevocations;

public record PermitRevocationCreateDto : PermitRevocationCommandDto, IWorkEntryCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
