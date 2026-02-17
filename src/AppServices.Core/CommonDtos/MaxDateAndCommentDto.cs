namespace AirWeb.AppServices.Core.CommonDtos;

// Used for adding a note or comment for various actions, such as creating, closing, or deleting Entities.
public record MaxDateAndCommentDto : MaxDateOnlyDto
{
    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string? Comment { get; init; }
}

public class MaxDateAndCommentValidator : BaseMaxCurrentDateValidator<MaxDateAndCommentDto>;
