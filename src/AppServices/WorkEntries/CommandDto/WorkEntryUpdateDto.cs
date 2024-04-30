using System.ComponentModel.DataAnnotations;

namespace MyApp.AppServices.WorkEntries.CommandDto;

public record WorkEntryUpdateDto : IWorkEntryCommandDto
{
    // Authorization handler assist properties
    public bool WorkEntryClosed { get; init; }
    public bool IsDeleted { get; init; }

    // Data
    public Guid EntryTypeId { get; init; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string Notes { get; init; } = string.Empty;
}
