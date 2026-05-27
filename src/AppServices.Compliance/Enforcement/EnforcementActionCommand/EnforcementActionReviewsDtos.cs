using AirWeb.AppServices.Core.CommonDtos;
using AirWeb.AppServices.Core.Utilities;
using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.Domain.Core.Data.DataAttributes;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.Enforcement.EnforcementActionCommand;

public record EnforcementActionRequestReviewDto
{
    [Required(ErrorMessage = "A reviewer must be selected.")]
    [Display(Name = "Request review from")]
    public string? RequestedOfId { get; init; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Date Requested")]
    [MaxDate]
    public DateOnly RequestedDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);
}

public class ReviewRequestValidator : AbstractValidator<EnforcementActionRequestReviewDto>
{
    public ReviewRequestValidator()
    {
        RuleFor(dto => dto.RequestedOfId)
            .NotEmpty()
            .WithMessage("A reviewer must be selected.");

        RuleFor(dto => dto.RequestedDate)
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The Date Requested cannot be in the future.");
    }
}

public record EnforcementActionSubmitReviewDto : NotesDto
{
    [Required(ErrorMessage = "A review result must be selected.")]
    public ReviewResult? Result { get; init; }

    [Display(Name = "Request additional review from")]
    public string? RequestedOfId { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Date additional review requested")]
    [MaxDate]
    public DateOnly? RequestedDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);
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

        RuleFor(dto => dto.RequestedDate)
            .NotNull()
            .When(dto => dto.Result == ReviewResult.Forwarded)
            .WithMessage("Requested date must be set.");
    }
}
