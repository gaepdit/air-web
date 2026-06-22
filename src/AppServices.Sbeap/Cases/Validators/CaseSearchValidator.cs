using AirWeb.AppServices.Sbeap.Cases.Dto;
using FluentValidation;

namespace AirWeb.AppServices.Sbeap.Cases.Validators;

public class CaseSearchValidator : AbstractValidator<CaseworkSearchDto>
{
    public CaseSearchValidator()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        RuleFor(dto => dto.ClosedFrom)
            .Must(date => date <= today || date == null)
            .WithMessage("The beginning closed date cannot be in the future");

        RuleFor(dto => dto.ClosedThrough)
            .Must((dto, date) => date >= dto.ClosedFrom)
            .When(dto => dto.ClosedFrom.HasValue && dto.ClosedThrough.HasValue)
            .WithMessage("The end closed date must be later than the beginning date");
        RuleFor(dto => dto.ReferredFrom)
            .Must(date => date <= today || date == null)
            .WithMessage("The beginning referred date cannot be in the future");

        RuleFor(dto => dto.ReferredThrough)
            .Must((dto, date) => date >= dto.ReferredFrom)
            .When(dto => dto.ReferredFrom.HasValue && dto.ReferredThrough.HasValue)
            .WithMessage("The end referred date must be later than the beginning date");
    }
}
