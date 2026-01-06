using AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.PermitRevocations;

public class PermitRevocationUpdateValidator : AbstractValidator<PermitRevocationUpdateDto>
{
    public PermitRevocationUpdateValidator(
        IValidator<IComplianceWorkCommandDto> complianceWorkCommandValidator,
        IValidator<PermitRevocationCommandDto> permitRevocationCommandValidator)
    {
        RuleFor(dto => dto).SetValidator(complianceWorkCommandValidator);
        RuleFor(dto => dto).SetValidator(permitRevocationCommandValidator);
    }
}
