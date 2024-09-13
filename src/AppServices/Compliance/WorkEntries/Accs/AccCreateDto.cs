using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.Accs;

public record AccCreateDto : AccCommandDto, IWorkEntryCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
