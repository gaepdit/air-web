using System.ComponentModel.DataAnnotations;

namespace MyApp.AppServices.WorkEntries.CommandDto;

// Used for closing, reopening, deleting, and restoring WorkEntries.
public record WorkEntryChangeStatusDto(Guid WorkEntryId)
{
    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string? Comments { get; init; } = string.Empty;
}
