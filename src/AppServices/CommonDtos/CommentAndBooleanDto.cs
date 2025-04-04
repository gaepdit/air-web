namespace AirWeb.AppServices.CommonDtos;

public record CommentAndBooleanDto
{
    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string? Comment { get; init; }

    public bool Option { get; init; }
}
