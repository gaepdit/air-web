using AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Command;

namespace AirWeb.AppServices.Compliance.ComplianceWork.PermitRevocations;

public record PermitRevocationCreateDto : PermitRevocationCommandDto, IWorkEntryCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
