using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;

public class PermitRevocationCreateValidator : AbstractValidator<PermitRevocationCreateDto>
{
    public PermitRevocationCreateValidator()
    {
        RuleFor(dto => dto).SetValidator(new WorkEntryCreateValidator());
        RuleFor(dto => dto).SetValidator(new PermitRevocationCommandValidator());
    }
}
