using AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Command;

namespace AirWeb.AppServices.Compliance.ComplianceWork.Accs;

public record AccCreateDto : AccCommandDto, IWorkEntryCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
