using FluentValidation;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

public class ComplianceWorkCreateValidator : AbstractValidator<IComplianceWorkCreateDto>
{
    public ComplianceWorkCreateValidator(IValidator<IComplianceWorkCommandDto> complianceWorkCommandValidator)
    {
        RuleFor(dto => dto.FacilityId).NotEmpty();
        RuleFor(dto => dto).SetValidator(complianceWorkCommandValidator);
    }
}
