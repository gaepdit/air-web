namespace AirWeb.AppServices.Core.EntityServices.Comments;

public record CommentAddDto(int ItemId)
{
    [Required(AllowEmptyStrings = false)]
    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Comment")]
    public string Comment { get; init; } = string.Empty;
}
