namespace AirWeb.AppServices.Core.CommonDtos;

// Used for adding an optional note or comment for various actions, such as creating, closing, or deleting Entities.
public record CommentDto
{
    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string? Comment { get; init; }
}
