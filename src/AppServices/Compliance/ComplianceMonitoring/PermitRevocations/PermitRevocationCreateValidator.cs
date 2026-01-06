using AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.PermitRevocations;

public class PermitRevocationCreateValidator : AbstractValidator<PermitRevocationCreateDto>
{
    public PermitRevocationCreateValidator(
        IValidator<IComplianceWorkCreateDto> complianceWorkCreateValidator,
        IValidator<PermitRevocationCommandDto> permitRevocationCommandValidator)
    {
        RuleFor(dto => dto).SetValidator(complianceWorkCreateValidator);
        RuleFor(dto => dto).SetValidator(permitRevocationCommandValidator);
    }
}
