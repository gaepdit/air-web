using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.WorkEntries.CommandDto;

public record WorkEntryUpdateDto : IWorkEntryUpdateDto, IWorkEntryCreateDto
{
    public bool IsClosed { get; init; }
    public bool IsDeleted { get; init; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string Notes { get; init; } = string.Empty;
}
