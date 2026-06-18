using IaipDataService.Facilities;
using AirWeb.AppServices.Sbeap.Cases.Dto;
using FluentValidation;

namespace AirWeb.AppServices.Sbeap.Cases.Validators;

public class CaseSearchValidator : AbstractValidator<CaseworkSearchDto>
{
    public CaseSearchValidator(IFacilityService service)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        RuleFor(dto => dto.ReferredFrom)
            .Must(date => date <= today || date == null)
            .WithMessage("The beginning referred date cannot be in the future");

        RuleFor(dto => dto.ReferredThrough)
            .Must((dto, date) => date >= dto.ReferredFrom)
            .When(dto => dto.ReferredFrom.HasValue && dto.ReferredThrough.HasValue)
            .WithMessage("The end referred date must be later than the beginning date");
    }
}
