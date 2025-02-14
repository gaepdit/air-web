namespace AirWeb.AppServices.CommonDtos;

// Used for adding a note or comment for various actions, such as creating, closing, or deleting Entities.
public record CommentDto
{
    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string? Comment { get; init; }
}
