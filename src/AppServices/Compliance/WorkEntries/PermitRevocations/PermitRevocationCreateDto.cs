using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;

namespace AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;

public record PermitRevocationCreateDto : PermitRevocationCommandDto, IWorkEntryCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
