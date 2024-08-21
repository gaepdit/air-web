using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Comments;

public record CommentAddDto<TKey>(TKey Id)
    where TKey : IEquatable<TKey>
{
    [Required(AllowEmptyStrings = false)]
    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Comment")]
    public required string Comment { get; init; }
}
