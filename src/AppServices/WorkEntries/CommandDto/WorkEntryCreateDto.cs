using System.ComponentModel.DataAnnotations;

namespace MyApp.AppServices.WorkEntries.CommandDto;

public record WorkEntryCreateDto : IWorkEntryCommandDto
{
    public Guid EntryTypeId { get; init; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string Notes { get; init; } = string.Empty;
}
