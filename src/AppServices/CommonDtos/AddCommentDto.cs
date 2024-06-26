using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.CommonDtos;

public record AddCommentDto<TKey>(TKey Id)
    where TKey : IEquatable<TKey>
{
    [Required(AllowEmptyStrings = false)]
    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Comment")]
    public string Comment { get; init; } = string.Empty;
}
