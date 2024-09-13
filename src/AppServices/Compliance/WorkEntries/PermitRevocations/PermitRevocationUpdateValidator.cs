using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;

public class PermitRevocationUpdateValidator : AbstractValidator<PermitRevocationUpdateDto>
{
    public PermitRevocationUpdateValidator()
    {
        RuleFor(dto => dto).SetValidator(new WorkEntryCommandValidator());
        RuleFor(dto => dto).SetValidator(new PermitRevocationCommandValidator());
    }
}
