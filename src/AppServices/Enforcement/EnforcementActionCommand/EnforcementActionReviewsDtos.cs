using AirWeb.AppServices.Core.CommonDtos;
using AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;
using FluentValidation;

namespace AirWeb.AppServices.Enforcement.EnforcementActionCommand;

public record EnforcementActionRequestReviewDto
{
    [Required(ErrorMessage = "A reviewer must be selected.")]
    [Display(Name = "Request review from")]
    public string? RequestedOfId { get; init; }
}

public class ReviewRequestValidator : AbstractValidator<EnforcementActionRequestReviewDto>
{
    public ReviewRequestValidator()
    {
        RuleFor(dto => dto.RequestedOfId)
            .NotEmpty()
            .WithMessage("A reviewer must be selected.");
    }
}

public record EnforcementActionSubmitReviewDto : CommentDto
{
    [Required(ErrorMessage = "A review result must be selected.")]
    public ReviewResult? Result { get; init; }

    [Display(Name = "Request additional review from")]
    public string? RequestedOfId { get; init; }
}

public class SubmitReviewValidator : AbstractValidator<EnforcementActionSubmitReviewDto>
{
    public SubmitReviewValidator()
    {
        RuleFor(dto => dto.Result)
            .NotNull();

        RuleFor(dto => dto.RequestedOfId)
            .NotEmpty()
            .When(dto => dto.Result == ReviewResult.Forwarded)
            .WithMessage("A reviewer must be selected.");
    }
}
