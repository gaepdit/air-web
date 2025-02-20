using FluentValidation;

namespace AirWeb.AppServices.CommonDtos;

public class MaxCurrentDateOnlyValidator : AbstractValidator<MaxCurrentDateOnlyDto>
{
    public MaxCurrentDateOnlyValidator()
    {
        RuleFor(dto => dto.Date)
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The date cannot be in the future.");
    }
}
