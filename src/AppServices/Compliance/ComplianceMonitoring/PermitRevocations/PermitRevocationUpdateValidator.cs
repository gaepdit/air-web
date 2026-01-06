using AirWeb.AppServices.Compliance.ComplianceMonitoring.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.PermitRevocations;

public class PermitRevocationUpdateValidator : AbstractValidator<PermitRevocationUpdateDto>
{
    public PermitRevocationUpdateValidator(
        IValidator<IWorkEntryCommandDto> workEntryCommandValidator,
        IValidator<PermitRevocationCommandDto> permitRevocationCommandValidator)
    {
        RuleFor(dto => dto).SetValidator(workEntryCommandValidator);
        RuleFor(dto => dto).SetValidator(permitRevocationCommandValidator);
    }
}
