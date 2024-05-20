using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.WorkEntries.CommandDto;

public record WorkEntryCreateDto : IWorkEntryCreateDto
{
    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string Notes { get; init; } = string.Empty;
}
