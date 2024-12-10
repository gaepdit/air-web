namespace AirWeb.AppServices.CommonDtos;

// Used for closing, reopening, deleting, and restoring Entities.
public record StatusCommentDto
{
    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string? Comment { get; init; }
}
