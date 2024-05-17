using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.CommonDto;

// Used for closing, reopening, deleting, and restoring Entities.
public record ChangeEntityStatusDto<TKey>(TKey Id)
    where TKey : IEquatable<TKey>
{
    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string? Comment { get; init; } = string.Empty;
}
