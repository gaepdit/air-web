using FluentValidation;

namespace AirWeb.AppServices.CommonDtos;

// Used for adding a note or comment for various actions, such as creating, closing, or deleting Entities.
public record CommentAndMaxDateDto
{
    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string? Comment { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly Date { get; init; } = DateOnly.FromDateTime(DateTime.Today);
}

public class CommentAndMaxDateValidator : AbstractValidator<CommentAndMaxDateDto>
{
    public CommentAndMaxDateValidator()
    {
        RuleFor(dto => dto.Date)
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The date cannot be in the future.");
    }
}
