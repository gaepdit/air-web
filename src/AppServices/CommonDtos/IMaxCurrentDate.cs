using FluentValidation;

namespace AirWeb.AppServices.CommonDtos;

public interface IMaxCurrentDate
{
    public DateOnly Date { get; }
}

public abstract class BaseMaxCurrentDateValidator<T> : AbstractValidator<T>
    where T : IMaxCurrentDate
{
    protected BaseMaxCurrentDateValidator()
    {
        RuleFor(dto => dto.Date)
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The date cannot be in the future.");
    }
}
