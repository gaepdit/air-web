using System.ComponentModel.DataAnnotations;

namespace MyApp.AppServices.EntryActions.Dto;

public record EntryActionUpdateDto
{
    public Guid WorkEntryId { get; init; }

    [Required]
    [Display(Name = "Action Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ActionDate { get; init; }

    [Required]
    [DataType(DataType.MultilineText)]
    [StringLength(10_000)]
    public string Comments { get; init; } = string.Empty;
}
