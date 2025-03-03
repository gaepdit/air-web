using AirWeb.AppServices.Utilities;
using FluentValidation;

namespace AirWeb.AppServices.CommonDtos;

public record MaxCurrentDateOnlyDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly Date { get; init; } = DateOnly.FromDateTime(DateTime.Today);
}

public class MaxCurrentDateOnlyValidator : AbstractValidator<MaxCurrentDateOnlyDto>
{
    public MaxCurrentDateOnlyValidator()
    {
        RuleFor(dto => dto.Date)
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The date cannot be in the future.");
    }
}
