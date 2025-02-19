using FluentValidation;

namespace AirWeb.AppServices.CommonDtos;

public class MaxCurrentDateOnlyValidator : AbstractValidator<MaxCurrentDateOnlyDto>
{
    public MaxCurrentDateOnlyValidator()
    {
        RuleFor(dto => dto.Date)
            .Must(only => only <= DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("The date cannot be in the future.");
    }
}
