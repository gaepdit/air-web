using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;

namespace AirWeb.AppServices.Compliance.WorkEntries.Accs;

public record AccCreateDto : AccCommandDto, IWorkEntryCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
